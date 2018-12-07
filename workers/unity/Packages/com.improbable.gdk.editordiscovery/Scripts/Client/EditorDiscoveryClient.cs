using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

namespace Improbable.GDK.EditorDiscovery
{
    internal class EditorDiscoveryClient : ThreadHandle
    {
        private readonly EditorDiscoveryClientThread _editorDiscoveryClientThread;

        internal EditorDiscoveryClient(
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

            _editorDiscoveryClientThread = new EditorDiscoveryClientThread(
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
            _editorDiscoveryClientThread.Start();
        }

        public NetworkInterfaceInfo[] GetNetworkInterfaceInfos()
        {
            return _editorDiscoveryClientThread.GetNetworkInterfaceInfos();
        }

        private class EditorDiscoveryClientThread
        {
            private INetworkInterface[] _availableNetInterfaces;

            private readonly Dictionary<string, ClientNetworkInterface> _clientInterfaceThreadHandles
                =
                new Dictionary<string, ClientNetworkInterface>();

            private readonly ManualResetEvent _killTrigger;
            private readonly int _timeBetweenBroadcastsMs;
            private readonly int _packetReceiveTimeoutMs;
            private readonly int _staleServerResponseTimeMs;
            private readonly int _editorDiscoveryPort;
            private readonly int _networkInterfaceCheckInterval;
            private readonly INetworkInterface[] _additionalNetworkInterfaces;

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
                _editorDiscoveryPort = editorDiscoveryPort;
                _timeBetweenBroadcastsMs = timeBetweenBroadcastsMs;
                _packetReceiveTimeoutMs = packetReceiveTimeoutMs;
                _staleServerResponseTimeMs = staleServerResponseTimeMs;
                _networkInterfaceCheckInterval = networkInterfaceCheckInterval;
                _additionalNetworkInterfaces = additionalNetworkInterfaces;
                _killTrigger = killTrigger;
            }

            public void Start()
            {
                _availableNetInterfaces = new INetworkInterface[0];

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
                        if (_killTrigger.WaitOne(_networkInterfaceCheckInterval))
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
                    .Concat(_additionalNetworkInterfaces);

                _availableNetInterfaces = networkInterfaceWrappers
                    .Where(networkInterface =>
                    {
                        if (_availableNetInterfaces.Any(otherInterface => otherInterface.Id == networkInterface.Id))
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
                _availableNetInterfaces = _availableNetInterfaces
                    .Where(networkInterface =>
                    {
                        if (IsNetworkInterfaceSuitable(networkInterface))
                        {
                            return true;
                        }

                        var threadHandle = _clientInterfaceThreadHandles[networkInterface.Id];

                        threadHandle.Kill(true);

                        _clientInterfaceThreadHandles.Remove(networkInterface.Id);

                        return false;
                    }).ToArray();
            }

            private void KillAllThreads()
            {
                foreach (var clientInterfaceThreadHandle in _clientInterfaceThreadHandles.Values)
                {
                    clientInterfaceThreadHandle.Kill(true);
                }

                _clientInterfaceThreadHandles.Clear();
            }

            private void SpawnThreadForInterface(INetworkInterface networkInterface)
            {
                if (_clientInterfaceThreadHandles.ContainsKey(networkInterface.Id))
                {
                    return;
                }

                var listenThreadHandle = new ClientNetworkInterface(
                    networkInterface,
                    _editorDiscoveryPort,
                    _timeBetweenBroadcastsMs,
                    _packetReceiveTimeoutMs,
                    _staleServerResponseTimeMs,
                    true);

                listenThreadHandle.Start();

                _clientInterfaceThreadHandles[networkInterface.Id] = listenThreadHandle;
            }

            public NetworkInterfaceInfo[] GetNetworkInterfaceInfos()
            {
                return _clientInterfaceThreadHandles.Values
                    .Select(clientInterfaceThreadHandle => clientInterfaceThreadHandle.GetNetworkInterfaceInfo())
                    .ToArray();
            }
        }
    }
}
