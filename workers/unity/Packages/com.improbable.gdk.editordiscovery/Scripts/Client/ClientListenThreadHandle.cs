using System.Net;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientListenThreadHandle : ThreadHandle
    {
        private readonly ClientListenThread clientListenThread;

        public ClientListenThreadHandle(
            IPEndPoint listenEndPoint,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs
        )
        {
            clientListenThread =
                new ClientListenThread(
                    listenEndPoint,
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
    }
}
