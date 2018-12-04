using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ServerListenThreadHandle : ThreadHandle
    {
        private readonly ConcurrentQueue<string> serverNameQueue = new ConcurrentQueue<string>();

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
                serverNameQueue,
                dataPath,
                companyName,
                productName);
        }

        protected override void ThreadMethod()
        {
            serverListenThread.Start();
        }

        public void SetName(string newName)
        {
            if (IsKilled)
            {
                throw new Exception("This thread handle was killed.");
            }

            serverNameQueue.Enqueue(newName);
        }
    }
}
