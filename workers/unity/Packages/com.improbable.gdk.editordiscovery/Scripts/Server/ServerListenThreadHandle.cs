using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ServerListenThreadHandle
    {
        private readonly Thread thread;

        private readonly ConcurrentQueue<string> serverNameQueue = new ConcurrentQueue<string>();
        private readonly ManualResetEvent killTrigger = new ManualResetEvent(false);

        private bool isKilled;
        private bool isStarted;

        public ServerListenThreadHandle(string serverName, int editorDiscoveryPort, int packetReceiveTimeoutMs)
        {
            var dataPath = Application.dataPath;
            var companyName = Application.companyName;
            var productName = Application.productName;

            thread = new Thread(() =>
            {
                new ServerListenThread(
                    serverName,
                    editorDiscoveryPort,
                    packetReceiveTimeoutMs,
                    killTrigger, serverNameQueue,
                    dataPath,
                    companyName,
                    productName).Start();
            });
        }

        internal void Start()
        {
            if (isKilled)
            {
                throw new Exception("This thread handle was killed.");
            }

            if (isStarted)
            {
                throw new Exception("Cannot start a thread handle twice.");
            }

            thread.Start();

            isStarted = true;
        }

        public void SetName(string newName)
        {
            if (isKilled)
            {
                throw new Exception("This thread handle was killed.");
            }

            serverNameQueue.Enqueue(newName);
        }

        public void Kill(bool wait = false)
        {
            if (isKilled)
            {
                throw new Exception("This thread handle was already killed.");
            }

            killTrigger.Set();
            if (wait)
            {
                if (!thread.Join(1000))
                {
                    throw new Exception("Server did not die within 1 second of kill message.");
                }
            }

            isKilled = true;
        }
    }
}