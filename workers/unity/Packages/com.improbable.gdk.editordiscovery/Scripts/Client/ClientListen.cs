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
        private readonly ClientListenThread clientListenThread;

        public ClientListen(
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs
        )
        {
            clientListenThread =
                new ClientListenThread(
                    packetReceiveTimeoutMs,
                    staleServerResponseTimeMs,
                    KillTrigger);
        }

        protected override void ThreadMethod()
        {
            clientListenThread.ThreadMethod();
        }

        public void WaitForReady()
        {
            clientListenThread.ReadyTrigger.WaitOne();
        }

        public int GetPort()
        {
            return clientListenThread.Port;
        }

        public ServerInfo[] GetServerInfos()
        {
            return clientListenThread.GetServerInfos();
        }

        private class ClientListenThread
        {
            private readonly ManualResetEvent killTrigger;
            private readonly int packetReceiveTimeoutMs;
            private readonly int staleServerResponseTimeMs;
            private readonly List<ServerInfo> serverInfoList = new List<ServerInfo>();

            public readonly ManualResetEvent ReadyTrigger;

            public int Port;

            public ClientListenThread(
                int packetReceiveTimeoutMs,
                int staleServerResponseTimeMs,
                ManualResetEvent killTrigger
            )
            {
                this.killTrigger = killTrigger;
                this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
                this.staleServerResponseTimeMs = staleServerResponseTimeMs;

                ReadyTrigger = new ManualResetEvent(false);
            }

            public ServerInfo[] GetServerInfos()
            {
                lock (serverInfoList)
                {
                    var serverInfos = serverInfoList.ToArray();
                    Array.Sort(serverInfos, (a, b) =>
                    {
                        var serverNameSort = string.Compare(a.ServerResponse.serverName, b.ServerResponse.serverName,
                            StringComparison.Ordinal);
                        return serverNameSort == 0
                            ? string.Compare(a.ServerResponse.dataPath, b.ServerResponse.dataPath,
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
                    receiveClient.Client.SetSocketOption(SocketOptionLevel.Socket,
                        SocketOptionName.ReuseAddress, true);
                    receiveClient.Client.SetSocketOption(SocketOptionLevel.Socket,
                        SocketOptionName.Broadcast, true);
                    receiveClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0));

                    Port = ((IPEndPoint) receiveClient.Client.LocalEndPoint).Port;

                    ReadyTrigger.Set();

                    while (true)
                    {
                        var packetReceiver = new CancellablePacketReceiver(receiveClient,
                            packetReceiveTimeoutMs,
                            killTrigger);

                        while (true)
                        {
                            var receiveResult = packetReceiver.Poll(out var remoteEndPoint, out var receivedBytes);

                            if (receiveResult == CancellablePacketReceiver.PollResult.Success)
                            {
                                var response = Encoding.ASCII.GetString(receivedBytes);

                                var serverResponse = JsonUtility.FromJson<EditorDiscoveryResponse>(response);

                                var ipAddress = remoteEndPoint.Address;

                                var newServerInfo = new ServerInfo(ipAddress, serverResponse);
                                lock (serverInfoList)
                                {
                                    serverInfoList.RemoveAll(existingServerInfo =>
                                        existingServerInfo.IPAddress.Equals(newServerInfo.IPAddress)
                                        && existingServerInfo.ServerResponse.dataPath ==
                                        newServerInfo.ServerResponse.dataPath);
                                    serverInfoList.Add(newServerInfo);
                                }

                                break;
                            }

                            if (receiveResult == CancellablePacketReceiver.PollResult.TimedOut)
                            {
                                lock (serverInfoList)
                                {
                                    serverInfoList.RemoveAll(serverInfo =>
                                    {
                                        if (!((DateTime.Now - serverInfo.ResponseTime).TotalMilliseconds >
                                            staleServerResponseTimeMs))
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
