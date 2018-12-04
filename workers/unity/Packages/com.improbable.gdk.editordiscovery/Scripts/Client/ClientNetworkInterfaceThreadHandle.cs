using System.Net;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientNetworkInterfaceThreadHandle : ThreadHandle
    {
        private readonly ClientNetworkInterfaceThread clientInterfaceThread;

        public ClientNetworkInterfaceThreadHandle(
            IPAddress sendAddress,
            IPAddress clientListenAddress,
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            bool isBroadcast
        )
        {
            clientInterfaceThread = new ClientNetworkInterfaceThread(
                sendAddress,
                clientListenAddress,
                editorDiscoveryPort,
                timeBetweenBroadcastsMs,
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs,
                isBroadcast,
                KillTrigger);
        }

        protected override void ThreadMethod()
        {
            clientInterfaceThread.Start();
        }
    }
}
