using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class EditorDiscoveryServer : ThreadHandle
    {
        private readonly EditorDiscoveryServerThread editorDiscoveryServerThread;

        public EditorDiscoveryServer(string serverName, int editorDiscoveryPort, int packetReceiveTimeoutMs)
        {
            var dataPath = Application.dataPath;
            var companyName = Application.companyName;
            var productName = Application.productName;

            editorDiscoveryServerThread = new EditorDiscoveryServerThread(
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
            editorDiscoveryServerThread.ThreadMethod();
        }

        public void SetName(string newName)
        {
            editorDiscoveryServerThread.ServerName = newName;
        }

        private class EditorDiscoveryServerThread
        {
            private readonly int packetReceiveTimeoutMs;
            private readonly int editorDiscoveryPort;
            private readonly ManualResetEvent killTrigger;

            public string ServerName;

            private readonly string dataPath;
            private readonly string companyName;
            private readonly string productName;

            internal EditorDiscoveryServerThread(
                string serverName,
                int editorDiscoveryPort,
                int packetReceiveTimeoutMs,
                ManualResetEvent killTrigger,
                string dataPath,
                string companyName,
                string productName)
            {
                ServerName = serverName;
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
                    var socket = client.Client;

                    // Allows multiple server listen threads to listen on the same port
                    // e.g. multiple Unity editor instances in the same computer.
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                    var ipEndPoint = new IPEndPoint(IPAddress.Any, editorDiscoveryPort);
                    socket.Bind(ipEndPoint);

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
                            serverName = ServerName,
                            companyName = companyName,
                            productName = productName,
                            dataPath = dataPath,
                        };

                        ServerResponseThread.StartThread(serverInfo, new IPEndPoint(remoteEp.Address, responsePort));
                        return TickResult.ReceivedPacket;
                    }

                    if (receiveResult == CancellablePacketReceiver.PollResult.Cancelled)
                    {
                        client.Close();
                        receiveHandle.ForceEnd();

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
        }
    }
}
