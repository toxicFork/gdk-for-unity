using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientListen : ThreadHandle
    {
        private readonly ClientListenThread _clientListenThread;

        public ClientListen(
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs
        )
        {
            _clientListenThread =
                new ClientListenThread(
                    packetReceiveTimeoutMs,
                    staleServerResponseTimeMs,
                    KillTrigger);
        }

        protected override void ThreadMethod()
        {
            _clientListenThread.ThreadMethod();
        }

        public void WaitForReady()
        {
            _clientListenThread.ReadyTrigger.WaitOne();
        }

        public int GetPort()
        {
            return _clientListenThread.Port;
        }

        public ServerInfo[] GetServerInfos()
        {
            return _clientListenThread.GetServerInfos();
        }

        private class ClientListenThread
        {
            private readonly ManualResetEvent _killTrigger;
            private readonly int _packetReceiveTimeoutMs;
            private readonly int _staleServerResponseTimeMs;
            private readonly List<ServerInfo> _serverInfoList = new List<ServerInfo>();

            public readonly ManualResetEvent ReadyTrigger;

            public int Port;

            public ClientListenThread(
                int packetReceiveTimeoutMs,
                int staleServerResponseTimeMs,
                ManualResetEvent killTrigger
            )
            {
                _killTrigger = killTrigger;
                _packetReceiveTimeoutMs = packetReceiveTimeoutMs;
                _staleServerResponseTimeMs = staleServerResponseTimeMs;

                ReadyTrigger = new ManualResetEvent(false);
            }

            public ServerInfo[] GetServerInfos()
            {
                lock (_serverInfoList)
                {
                    var serverInfos = _serverInfoList.ToArray();
                    Array.Sort(serverInfos, (a, b) =>
                    {
                        var serverNameSort = string.Compare(a.ServerResponse.ServerName, b.ServerResponse.ServerName,
                            StringComparison.Ordinal);
                        return serverNameSort == 0
                            ? string.Compare(a.ServerResponse.DataPath, b.ServerResponse.DataPath,
                                StringComparison.Ordinal)
                            : serverNameSort;
                    });
                    return serverInfos;
                }
            }

            public void ThreadMethod()
            {
                using (var receiveClient = new UdpClient())
                {
                    // receiveClient.Client.SendTimeout = 200;
                    // receiveClient.Client.ReceiveTimeout = 200;

                    receiveClient.Client.SetSocketOption(SocketOptionLevel.Socket,
                        SocketOptionName.ReuseAddress, true);
                    receiveClient.Client.SetSocketOption(SocketOptionLevel.Socket,
                        SocketOptionName.Broadcast, true);
                    receiveClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0));

                    Port = ((IPEndPoint) receiveClient.Client.LocalEndPoint).Port;

                    // TODO start listen
                    ReadyTrigger.Set();

                    while (true)
                    {
                        var packetReceiver = new CancellablePacketReceiver(receiveClient,
                            _packetReceiveTimeoutMs,
                            _killTrigger);

                        while (true)
                        {
                            var receiveResult = packetReceiver.Poll(out var remoteEndPoint, out var receivedBytes);

                            if (receiveResult == CancellablePacketReceiver.PollResult.Success)
                            {
                                var response = Encoding.ASCII.GetString(receivedBytes);

                                var serverResponse = JsonUtility.FromJson<EditorDiscoveryResponse>(response);

                                var ipAddress = remoteEndPoint.Address;

                                var newServerInfo = new ServerInfo(ipAddress, serverResponse);
                                lock (_serverInfoList)
                                {
                                    _serverInfoList.RemoveAll(existingServerInfo =>
                                        existingServerInfo.IPAddress.Equals(newServerInfo.IPAddress)
                                        && existingServerInfo.ServerResponse.DataPath ==
                                        newServerInfo.ServerResponse.DataPath);
                                    _serverInfoList.Add(newServerInfo);
                                }

                                break;
                            }

                            if (receiveResult == CancellablePacketReceiver.PollResult.TimedOut)
                            {
                                lock (_serverInfoList)
                                {
                                    _serverInfoList.RemoveAll(serverInfo =>
                                    {
                                        if (!((DateTime.Now - serverInfo.ResponseTime).TotalMilliseconds >
                                            _staleServerResponseTimeMs))
                                        {
                                            return false;
                                        }

                                        return true;
                                    });
                                }

                                continue;
                            }

                            if (receiveResult == CancellablePacketReceiver.PollResult.Cancelled)
                            {
                                receiveClient.Close();
                                packetReceiver.ForceEnd();
                                return;
                            }

                            // Invalid receiveResult
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
        }
    }
}
