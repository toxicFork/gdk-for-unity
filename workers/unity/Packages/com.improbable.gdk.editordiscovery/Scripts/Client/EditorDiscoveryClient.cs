using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

namespace Improbable.GDK.EditorDiscovery
{
    public class EditorDiscoveryClient : ThreadHandle
    {
        private readonly EditorDiscoveryClientThread editorDiscoveryClientThread;

        public EditorDiscoveryClient(
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            int networkInterfaceCheckInterval,
            INetworkInterface[] additionalNetworkInterfaces = null
        )
        {
            if (additionalNetworkInterfaces == null)
            {
                additionalNetworkInterfaces = new INetworkInterface[0];
            }

            editorDiscoveryClientThread = new EditorDiscoveryClientThread(
                editorDiscoveryPort,
                timeBetweenBroadcastsMs,
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs,
                networkInterfaceCheckInterval,
                additionalNetworkInterfaces,
                KillTrigger);
        }

        protected override void ThreadMethod()
        {
            editorDiscoveryClientThread.Start();
        }

        public NetworkInterfaceInfo[] GetNetworkInterfaceInfos()
        {
            return editorDiscoveryClientThread.GetNetworkInterfaceInfos();
        }

        private class EditorDiscoveryClientThread
        {
            private INetworkInterface[] availableNetInterfaces;

            private readonly Dictionary<string, ClientNetworkInterface> clientInterfaceThreadHandles
                =
                new Dictionary<string, ClientNetworkInterface>();

            private readonly ManualResetEvent killTrigger;
            private readonly int timeBetweenBroadcastsMs;
            private readonly int packetReceiveTimeoutMs;
            private readonly int staleServerResponseTimeMs;
            private readonly int editorDiscoveryPort;
            private readonly int networkInterfaceCheckInterval;
            private readonly INetworkInterface[] additionalNetworkInterfaces;

            public EditorDiscoveryClientThread(
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

                    try
                    {
                        if (killTrigger.WaitOne(networkInterfaceCheckInterval))
                        {
                            KillAllThreads();
                            return;
                        }
                    }
                    catch (ThreadAbortException)
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
                        if (availableNetInterfaces.Any(otherInterface => otherInterface.Id == networkInterface.Id))
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

                        var threadHandle = clientInterfaceThreadHandles[networkInterface.Id];

                        threadHandle.Kill(true);

                        clientInterfaceThreadHandles.Remove(networkInterface.Id);

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
                if (clientInterfaceThreadHandles.ContainsKey(networkInterface.Id))
                {
                    return;
                }

                var listenThreadHandle = new ClientNetworkInterface(
                    networkInterface,
                    editorDiscoveryPort,
                    timeBetweenBroadcastsMs,
                    packetReceiveTimeoutMs,
                    staleServerResponseTimeMs,
                    true);

                listenThreadHandle.Start();

                clientInterfaceThreadHandles[networkInterface.Id] = listenThreadHandle;
            }

            public NetworkInterfaceInfo[] GetNetworkInterfaceInfos()
            {
                return clientInterfaceThreadHandles.Values
                    .Select(clientInterfaceThreadHandle => clientInterfaceThreadHandle.GetNetworkInterfaceInfo())
                    .ToArray();
            }
        }
    }
}
