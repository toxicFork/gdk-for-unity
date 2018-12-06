using System.Net;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientNetworkInterfaceThreadHandle : ThreadHandle
    {
        private readonly ClientNetworkInterfaceThread clientInterfaceThread;

        public ClientNetworkInterfaceThreadHandle(
            IPAddress bindingAddress,
            IPAddress sendAddress,
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            bool allowRouting
        )
        {
            clientInterfaceThread = new ClientNetworkInterfaceThread(
                bindingAddress,
                sendAddress,
                editorDiscoveryPort,
                timeBetweenBroadcastsMs,
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs,
                allowRouting,
                KillTrigger);
        }

        protected override void ThreadMethod()
        {
            clientInterfaceThread.Start();
        }
    }
}
