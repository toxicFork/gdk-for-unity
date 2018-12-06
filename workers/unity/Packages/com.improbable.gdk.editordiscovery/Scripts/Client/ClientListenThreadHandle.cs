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
            clientListenThread.ThreadMethod();
        }

        public void WaitForReady()
        {
            clientListenThread.ReadyTrigger.WaitOne();
        }

        public int GetPort()
        {
            return clientListenThread.port;
        }

        public ServerInfo[] GetServerInfos()
        {
            return clientListenThread.GetServerInfos();
        }
    }
}
