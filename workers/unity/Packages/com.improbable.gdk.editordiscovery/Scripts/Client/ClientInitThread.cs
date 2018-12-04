using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientInitThread
    {
        private NetworkInterface[] availableNetInterfaces;

        private readonly Dictionary<NetworkInterface, ClientNetworkInterfaceThreadHandle> clientInterfaceThreadHandles =
            new Dictionary<NetworkInterface, ClientNetworkInterfaceThreadHandle>();

        private readonly ManualResetEvent killTrigger;
        private readonly int timeBetweenBroadcastsMs;
        private readonly int packetReceiveTimeoutMs;
        private readonly int staleServerResponseTimeMs;
        private readonly int editorDiscoveryPort;

        public ClientInitThread(
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            ManualResetEvent killTrigger
        )
        {
            this.editorDiscoveryPort = editorDiscoveryPort;
            this.timeBetweenBroadcastsMs = timeBetweenBroadcastsMs;
            this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
            this.staleServerResponseTimeMs = staleServerResponseTimeMs;
            this.killTrigger = killTrigger;
        }

        public void Start()
        {
            availableNetInterfaces = new NetworkInterface[0];

            Loop();
        }

        private void Loop()
        {
            while (true)
            {
                if (killTrigger.WaitOne(0))
                {
                    KillAllThreads();
                    return;
                }

                CheckForDeadNetworkInterfaces();
                CheckForNewNetworkInterfaces();
            }
        }

        private void CheckForNewNetworkInterfaces()
        {
            availableNetInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(networkInterface =>
                {
                    if (availableNetInterfaces.Contains(networkInterface))
                    {
                        return true;
                    }

                    if (!IsNetworkInterfaceSuitable(networkInterface))
                    {
                        return false;
                    }

                    SpawnThreadForInterface(networkInterface);
                    return true;
                }).ToArray();
        }

        private static bool IsNetworkInterfaceSuitable(NetworkInterface networkInterface)
        {
            if (networkInterface.OperationalStatus == OperationalStatus.LowerLayerDown ||
                networkInterface.OperationalStatus == OperationalStatus.Down ||
                networkInterface.OperationalStatus == OperationalStatus.NotPresent)
            {
                return false;
            }

            if (networkInterface.GetIPProperties().UnicastAddresses.Count == 0)
            {
                return false;
            }

            if (networkInterface.GetIPProperties().UnicastAddresses.Count > 1)
            {
                Debug.LogWarning("multiple unicast addresses?");
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

        private void SpawnThreadForInterface(NetworkInterface availableNetInterface)
        {
            var sendAddress = availableNetInterface.GetIPProperties().UnicastAddresses.First().Address;

            var listenThreadHandle = new ClientNetworkInterfaceThreadHandle(
                sendAddress,
                IPAddress.Any /* TODO is it any? */,
                editorDiscoveryPort,
                timeBetweenBroadcastsMs,
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs,
                true);

            listenThreadHandle.Start();

            clientInterfaceThreadHandles[availableNetInterface] = listenThreadHandle;
        }
    }
}
