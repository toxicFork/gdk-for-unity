using System.Net;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientNetworkInterfaceThreadHandle : ThreadHandle
    {
        private readonly ClientNetworkInterfaceThread clientInterfaceThread;

        public ClientNetworkInterfaceThreadHandle(
            INetworkInterface networkInterface,
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            bool allowRouting
        )
        {
            clientInterfaceThread = new ClientNetworkInterfaceThread(
                networkInterface,
                editorDiscoveryPort,
                timeBetweenBroadcastsMs,
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs,
                allowRouting,
                KillTrigger);
        }

        protected override void ThreadMethod()
        {
            clientInterfaceThread.ThreadMethod();
        }

        private ServerInfo[] GetServerInfos()
        {
            return clientInterfaceThread.GetServerInfos();
        }

        public NetworkInterfaceInfo GetNetworkInterfaceInfo()
        {
            return clientInterfaceThread.GetNetworkInterfaceInfo();
        }
    }
}
