using System.Net;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientListenThreadHandle : ThreadHandle
    {
        private readonly ClientListenThread clientListenThread;

        public ClientListenThreadHandle(
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs
        )
        {
            clientListenThread =
                new ClientListenThread(
                    packetReceiveTimeoutMs,
                    staleServerResponseTimeMs,
                    KillTrigger);
        }

        protected override void ThreadMethod()
        {
            clientListenThread.Start();
        }

        public void WaitForReady()
        {
            clientListenThread.ReadyTrigger.WaitOne();
        }

        public int GetPort()
        {
            return clientListenThread.port;
        }
    }
}
