namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientInitThreadHandle : ThreadHandle
    {
        private readonly ClientInitThread clientInitThread;

        internal ClientInitThreadHandle(
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

            clientInitThread = new ClientInitThread(
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
            clientInitThread.Start();
        }

        public NetworkInterfaceInfo[] GetNetworkInterfaceInfos()
        {
            return clientInitThread.GetNetworkInterfaceInfos();
        }
    }
}
