using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ServerListenThreadHandle : ThreadHandle
    {
        private readonly ServerListenThread serverListenThread;

        public ServerListenThreadHandle(string serverName, int editorDiscoveryPort, int packetReceiveTimeoutMs)
        {
            var dataPath = Application.dataPath;
            var companyName = Application.companyName;
            var productName = Application.productName;

            serverListenThread = new ServerListenThread(
                serverName,
                editorDiscoveryPort,
                packetReceiveTimeoutMs,
                KillTrigger,
                dataPath,
                companyName,
                productName);
        }

        protected override void ThreadMethod()
        {
            serverListenThread.ThreadMethod();
        }

        public void SetName(string newName)
        {
            serverListenThread.serverName = newName;
        }
    }
}
