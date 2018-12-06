using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ServerListenThread
    {
        private readonly int packetReceiveTimeoutMs;
        private readonly int editorDiscoveryPort;
        private readonly ManualResetEvent killTrigger;

        public string serverName;

        private readonly string dataPath;
        private readonly string companyName;
        private readonly string productName;

        internal ServerListenThread(
            string serverName,
            int editorDiscoveryPort,
            int packetReceiveTimeoutMs,
            ManualResetEvent killTrigger,
            string dataPath,
            string companyName,
            string productName)
        {
            this.serverName = serverName;
            this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
            this.editorDiscoveryPort = editorDiscoveryPort;
            this.killTrigger = killTrigger;

            this.dataPath = dataPath;
            this.companyName = companyName;
            this.productName = productName;
        }

        internal void ThreadMethod()
        {
            using (var client = new UdpClient())
            {
                // client.Client.SendTimeout = 200;
                // client.Client.ReceiveTimeout = 200;

                var socket = client.Client;

                // Allows multiple server listen threads to listen on the same port
                // e.g. multiple Unity editor instances in the same computer.
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                var ipEndPoint = new IPEndPoint(IPAddress.Any, editorDiscoveryPort);
                socket.Bind(ipEndPoint);

                Debug.Log($"Server listening at {ipEndPoint}");

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

                if (receiveResult == CancellablePacketReceiver.PollResult.Success)
                {
                    var receivedString = Encoding.ASCII.GetString(receivedBytes);
                    Debug.Log(
                        $">>>>> Rec: {receivedString} from {remoteEp.Address} {remoteEp.Port}");

                    int responsePort;

                    try
                    {
                        responsePort = JsonUtility.FromJson<ClientRequest>(receivedString).listenPort;
                    }
                    catch (Exception)
                    {
                        responsePort = remoteEp.Port;
                    }

                    var serverInfo = new EditorDiscoveryResponse
                    {
                        ServerName = serverName,
                        CompanyName = companyName,
                        ProductName = productName,
                        DataPath = dataPath,
                    };

                    ServerResponseThread.StartThread(serverInfo, new IPEndPoint(remoteEp.Address, responsePort));
                    return TickResult.ReceivedPacket;
                }

                if (receiveResult == CancellablePacketReceiver.PollResult.Cancelled)
                {
                    // TODO handle kill

                    client.Close();
                    receiveHandle.ForceEnd();

                    Debug.Log("Killed?");
                    return TickResult.Killed;
                }

                if (receiveResult == CancellablePacketReceiver.PollResult.TimedOut)
                {
                    continue;
                }

                // Unknown receive result
                throw new ArgumentOutOfRangeException();
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
