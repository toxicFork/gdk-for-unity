using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
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


    internal class ServerListenThread
    {
        private readonly int packetReceiveTimeoutMs;
        private readonly int editorDiscoveryPort;
        private readonly ManualResetEvent killTrigger;
        private readonly ConcurrentQueue<string> serverNameQueue;

        private string serverName;

        private readonly string dataPath;
        private readonly string companyName;
        private readonly string productName;

        internal ServerListenThread(
            string serverName,
            int editorDiscoveryPort,
            int packetReceiveTimeoutMs,
            ManualResetEvent killTrigger,
            ConcurrentQueue<string> serverNameQueue,
            string dataPath,
            string companyName,
            string productName)
        {
            this.serverName = serverName;
            this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
            this.editorDiscoveryPort = editorDiscoveryPort;
            this.killTrigger = killTrigger;
            this.serverNameQueue = serverNameQueue;

            this.dataPath = dataPath;
            this.companyName = companyName;
            this.productName = productName;
        }

        internal void Start()
        {
            using (var client = new UdpClient())
            {
                var socket = client.Client;

                // Allows multiple server listen threads to listen on the same port
                // e.g. multiple Unity editor instances in the same computer.
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socket.Bind(new IPEndPoint(IPAddress.Any, editorDiscoveryPort));

                try
                {
                    while (true)
                    {
                        var tickResult = Tick(client);

                        if (tickResult == TickResult.ReceivedPacket)
                        {
                            continue;
                        }

                        if (tickResult == TickResult.Killed)
                        {
                            return;
                        }

                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private enum TickResult
        {
            ReceivedPacket,
            Killed
        }

        private TickResult Tick(UdpClient client)
        {
            var receiveHandle = new CancellablePacketReceiver(client, packetReceiveTimeoutMs, killTrigger);

            // Wait for a packet or a kill
            while (true)
            {
                var receiveResult = receiveHandle.Poll(out var remoteEp, out var receivedBytes);

                if (receiveResult == CancellablePacketReceiver.ReceiveResult.Success)
                {
                    Debug.Log(
                        $">>>>> Rec: {Encoding.ASCII.GetString(receivedBytes)} from {remoteEp.Address} {remoteEp.Port}");

                    UpdateServerName();

                    var serverInfo = new EditorDiscoveryResponse
                    {
                        ServerName = serverName,
                        CompanyName = companyName,
                        ProductName = productName,
                        DataPath = dataPath,
                    };

                    ServerResponseThread.StartThread(serverInfo, remoteEp);
                    return TickResult.ReceivedPacket;
                }

                if (receiveResult == CancellablePacketReceiver.ReceiveResult.Cancelled)
                {
                    // TODO handle kill

                    client.Close();
                    receiveHandle.ForceEnd();

                    Debug.Log("Killed?");
                    return TickResult.Killed;
                }

                if (receiveResult == CancellablePacketReceiver.ReceiveResult.TimedOut)
                {
                    continue;
                }

                // Unknown receive result
                throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateServerName()
        {
            // TODO use a mutex instead?
            // TODO google "thread-safe string C#"
            while (!serverNameQueue.IsEmpty)
            {
                if (serverNameQueue.TryDequeue(out var newServerName))
                {
                    serverName = newServerName;
                }
            }
        }

        public static ServerListenThreadHandle StartThread(string serverName, int editorDiscoveryPort,
            int packetReceiveTimeoutMs)
        {
            var handle = new ServerListenThreadHandle(serverName, editorDiscoveryPort, packetReceiveTimeoutMs);

            handle.Start();

            return handle;
        }
    }
}
