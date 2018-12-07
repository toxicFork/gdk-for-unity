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
        private readonly EditorDiscoveryServerThread _editorDiscoveryServerThread;

        public EditorDiscoveryServer(string serverName, int editorDiscoveryPort, int packetReceiveTimeoutMs)
        {
            var dataPath = Application.dataPath;
            var companyName = Application.companyName;
            var productName = Application.productName;

            _editorDiscoveryServerThread = new EditorDiscoveryServerThread(
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
            _editorDiscoveryServerThread.ThreadMethod();
        }

        public void SetName(string newName)
        {
            _editorDiscoveryServerThread.ServerName = newName;
        }

        private class EditorDiscoveryServerThread
        {
            private readonly int _packetReceiveTimeoutMs;
            private readonly int _editorDiscoveryPort;
            private readonly ManualResetEvent _killTrigger;

            public string ServerName;

            private readonly string _dataPath;
            private readonly string _companyName;
            private readonly string _productName;

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
                _packetReceiveTimeoutMs = packetReceiveTimeoutMs;
                _editorDiscoveryPort = editorDiscoveryPort;
                _killTrigger = killTrigger;

                _dataPath = dataPath;
                _companyName = companyName;
                _productName = productName;
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
                    var ipEndPoint = new IPEndPoint(IPAddress.Any, _editorDiscoveryPort);
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
                var receiveHandle = new CancellablePacketReceiver(client, _packetReceiveTimeoutMs, _killTrigger);

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
                            responsePort = JsonUtility.FromJson<ClientRequest>(receivedString).ListenPort;
                        }
                        catch (Exception)
                        {
                            responsePort = remoteEp.Port;
                        }

                        var serverInfo = new EditorDiscoveryResponse
                        {
                            ServerName = ServerName,
                            CompanyName = _companyName,
                            ProductName = _productName,
                            DataPath = _dataPath,
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
