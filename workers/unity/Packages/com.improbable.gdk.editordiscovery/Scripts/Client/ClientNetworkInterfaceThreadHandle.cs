using System.Net;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientNetworkInterfaceThreadHandle : ThreadHandle
    {
        private readonly ClientNetworkInterfaceThread clientInterfaceThread;

        public ClientNetworkInterfaceThreadHandle(
            IPAddress sendAddress,
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            bool isBroadcast
        )
        {
            clientInterfaceThread = new ClientNetworkInterfaceThread(
                sendAddress,
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
