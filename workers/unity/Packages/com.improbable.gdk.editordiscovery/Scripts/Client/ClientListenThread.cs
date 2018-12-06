using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientListenThread
    {
        private readonly ManualResetEvent killTrigger;
        public readonly ManualResetEvent ReadyTrigger;
        private readonly int packetReceiveTimeoutMs;
        private readonly int staleServerResponseTimeMs;

        private readonly List<ServerInfo> serverInfoList = new List<ServerInfo>();

        public int port;

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
            var serverInfos = serverInfoList.ToArray();
            Array.Sort(serverInfos, (a, b) =>
            {
                var serverNameSort = string.Compare(a.serverResponse.ServerName, b.serverResponse.ServerName,
                    StringComparison.Ordinal);
                return serverNameSort == 0
                    ? string.Compare(a.serverResponse.DataPath, b.serverResponse.DataPath, StringComparison.Ordinal)
                    : serverNameSort;
            });
            return serverInfos;
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

                port = ((IPEndPoint) receiveClient.Client.LocalEndPoint).Port;

                // TODO start listen
                ReadyTrigger.Set();

                while (true)
                {
                    Debug.Log($"Receiving!");

                    var packetReceiver = new CancellablePacketReceiver(receiveClient,
                        packetReceiveTimeoutMs,
                        killTrigger);

                    while (true)
                    {
                        var receiveResult = packetReceiver.Poll(out var remoteEndPoint, out var receivedBytes);

                        if (receiveResult == CancellablePacketReceiver.PollResult.Success)
                        {
                            var response = Encoding.ASCII.GetString(receivedBytes);

                            Debug.Log($"Received response: " + response);

                            var serverResponse = JsonUtility.FromJson<EditorDiscoveryResponse>(response);

                            var ipAddress = remoteEndPoint.Address;

                            var newServerInfo = new ServerInfo(ipAddress, serverResponse);
                            serverInfoList.RemoveAll(existingServerInfo =>
                                existingServerInfo.IPAddress.Equals(newServerInfo.IPAddress)
                                && existingServerInfo.serverResponse.DataPath == newServerInfo.serverResponse.DataPath);
                            serverInfoList.Add(newServerInfo);
                            SignalServerListChanged();
                            break;
                        }

                        if (receiveResult == CancellablePacketReceiver.PollResult.TimedOut)
                        {
                            serverInfoList.RemoveAll(serverInfo =>
                            {
                                if (!((DateTime.Now - serverInfo.ResponseTime).TotalMilliseconds >
                                    staleServerResponseTimeMs))
                                {
                                    return false;
                                }

                                SignalServerListChanged();
                                return true;
                            });
                            continue;
                        }

                        if (receiveResult == CancellablePacketReceiver.PollResult.Cancelled)
                        {
                            Debug.Log($"Cancelled?");
                            receiveClient.Close();
                            packetReceiver.ForceEnd();

                            Debug.Log($"Killed?");
                            // TODO quit
                            return;
                        }

                        Debug.LogError($"Invalid receiveResult?");

                        // Invalid receiveResult
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        private void SignalServerListChanged()
        {
            // TODO
            Debug.Log("Server infos:");
            foreach (var serverInfo in serverInfoList)
            {
                Debug.Log(
                    $"Server info {serverInfo.IPAddress}: {JsonUtility.ToJson(serverInfo.serverResponse, true)}, {serverInfo.ResponseTime}");
            }
        }
    }
}
