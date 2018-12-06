using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientInitThread
    {
        private INetworkInterface[] availableNetInterfaces;

        private readonly Dictionary<INetworkInterface, ClientNetworkInterfaceThreadHandle> clientInterfaceThreadHandles
            =
            new Dictionary<INetworkInterface, ClientNetworkInterfaceThreadHandle>();

        private readonly ManualResetEvent killTrigger;
        private readonly int timeBetweenBroadcastsMs;
        private readonly int packetReceiveTimeoutMs;
        private readonly int staleServerResponseTimeMs;
        private readonly int editorDiscoveryPort;
        private readonly int networkInterfaceCheckInterval;
        private readonly INetworkInterface[] additionalNetworkInterfaces;

        public ClientInitThread(
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            int networkInterfaceCheckInterval,
            INetworkInterface[] additionalNetworkInterfaces,
            ManualResetEvent killTrigger
        )
        {
            this.editorDiscoveryPort = editorDiscoveryPort;
            this.timeBetweenBroadcastsMs = timeBetweenBroadcastsMs;
            this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
            this.staleServerResponseTimeMs = staleServerResponseTimeMs;
            this.networkInterfaceCheckInterval = networkInterfaceCheckInterval;
            this.additionalNetworkInterfaces = additionalNetworkInterfaces;
            this.killTrigger = killTrigger;
        }

        public void Start()
        {
            availableNetInterfaces = new INetworkInterface[0];

            Loop();
        }

        private void Loop()
        {
            while (true)
            {
                CheckForDeadNetworkInterfaces();
                CheckForNewNetworkInterfaces();

                Debug.Log("interfaces: " + string.Join(", ",
                    availableNetInterfaces.Select(networkInterface => networkInterface.Name)));

                if (killTrigger.WaitOne(networkInterfaceCheckInterval))
                {
                    KillAllThreads();
                    return;
                }
            }
        }

        private void CheckForNewNetworkInterfaces()
        {
            var networkInterfaceWrappers = NetworkInterface.GetAllNetworkInterfaces()
                .Select(networkInterface => (INetworkInterface) new NetworkInterfaceWrapper(networkInterface))
                .Concat(additionalNetworkInterfaces);

            availableNetInterfaces = networkInterfaceWrappers
                .Where(networkInterface =>
                {
                    Debug.Log($"Discovered: {networkInterface.Name}");

                    if (availableNetInterfaces.Any(otherInterface => otherInterface.Name == networkInterface.Name))
                    {
                        Debug.Log($"{networkInterface.Name} already in the list");

                        return true;
                    }

                    if (!IsNetworkInterfaceSuitable(networkInterface))
                    {
                        Debug.Log($"{networkInterface.Name} not suitable!");
                        return false;
                    }

                    Debug.Log($"{networkInterface.Name} suitable!");

                    SpawnThreadForInterface(networkInterface);
                    return true;
                }).ToArray();
        }

        private static bool IsNetworkInterfaceSuitable(INetworkInterface networkInterface)
        {
            if (networkInterface.OperationalStatus == OperationalStatus.LowerLayerDown ||
                networkInterface.OperationalStatus == OperationalStatus.Down ||
                networkInterface.OperationalStatus == OperationalStatus.NotPresent)
            {
                return false;
            }

            var bindingAddresses = networkInterface.GetBindingAddress();

            if (bindingAddresses == null)
            {
                return false;
            }

            return true;
        }

        private void CheckForDeadNetworkInterfaces()
        {
            availableNetInterfaces = availableNetInterfaces
                .Where(networkInterface =>
                {
                    if (IsNetworkInterfaceSuitable(networkInterface))
                    {
                        return true;
                    }

                    Debug.Log($"killing: {networkInterface.Name}");

                    var threadHandle = clientInterfaceThreadHandles[networkInterface];

                    threadHandle.Kill(true);

                    clientInterfaceThreadHandles.Remove(networkInterface);

                    return false;
                }).ToArray();
        }

        private void KillAllThreads()
        {
            foreach (var clientInterfaceThreadHandle in clientInterfaceThreadHandles.Values)
            {
                clientInterfaceThreadHandle.Kill(true);
            }

            clientInterfaceThreadHandles.Clear();
        }

        private void SpawnThreadForInterface(INetworkInterface networkInterface)
        {
            Debug.Log($"Spawning thread for interface: {networkInterface.Name}");
            var listenThreadHandle = new ClientNetworkInterfaceThreadHandle(
                networkInterface.GetBindingAddress(),
                networkInterface.GetSendAddress(),
                editorDiscoveryPort,
                timeBetweenBroadcastsMs,
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs,
                true);

            listenThreadHandle.Start();

            clientInterfaceThreadHandles[networkInterface] = listenThreadHandle;
        }
    }
}
