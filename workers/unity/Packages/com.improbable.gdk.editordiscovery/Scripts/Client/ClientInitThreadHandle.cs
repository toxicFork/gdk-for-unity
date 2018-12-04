namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientInitThreadHandle : ThreadHandle
    {
        private readonly ClientInitThread clientInitThread;

        internal ClientInitThreadHandle(
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs
        )
        {
            clientInitThread = new ClientInitThread(
                editorDiscoveryPort,
                timeBetweenBroadcastsMs,
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs,
                KillTrigger);
        }

        protected override void ThreadMethod()
        {
            clientInitThread.Start();
        }
    }
}
