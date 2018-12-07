using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientNetworkInterface : ThreadHandle
    {
        private readonly ClientNetworkInterfaceThread clientInterfaceThread;

        public ClientNetworkInterface(
            INetworkInterface networkInterface,
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            bool allowRouting
        )
        {
            clientInterfaceThread = new ClientNetworkInterfaceThread(
                networkInterface,
                editorDiscoveryPort,
                timeBetweenBroadcastsMs,
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs,
                allowRouting,
                KillTrigger);
        }

        protected override void ThreadMethod()
        {
            clientInterfaceThread.ThreadMethod();
        }

        public NetworkInterfaceInfo GetNetworkInterfaceInfo()
        {
            return clientInterfaceThread.GetNetworkInterfaceInfo();
        }

        private class ClientNetworkInterfaceThread
        {
            private readonly IPAddress bindAddress;
            private readonly IPAddress sendAddress;
            private readonly int editorDiscoveryPort;
            private readonly int timeBetweenBroadcastsMs;

            private readonly bool allowRouting;
            private readonly ManualResetEvent killTrigger;

            private int listenPort;
            private readonly ClientListen clientListen;

            private readonly INetworkInterface networkInterface;

            private DateTime lastSend;

            public ClientNetworkInterfaceThread(
                INetworkInterface networkInterface,
                int editorDiscoveryPort,
                int timeBetweenBroadcastsMs,
                int packetReceiveTimeoutMs,
                int staleServerResponseTimeMs,
                bool allowRouting,
                ManualResetEvent killTrigger)
            {
                this.networkInterface = networkInterface;
                bindAddress = networkInterface.GetBindingAddress();
                sendAddress = networkInterface.GetSendAddress();

                this.editorDiscoveryPort = editorDiscoveryPort;
                this.timeBetweenBroadcastsMs = timeBetweenBroadcastsMs;
                this.allowRouting = allowRouting;
                this.killTrigger = killTrigger;

                clientListen = new ClientListen(
                    packetReceiveTimeoutMs,
                    staleServerResponseTimeMs);
            }

            public void ThreadMethod()
            {
                clientListen.Start();
                clientListen.WaitForReady();
                listenPort = clientListen.GetPort();

                using (var udpClient = new UdpClient())
                {
                    // udpClient.Client.SendTimeout = 200;
                    // udpClient.Client.ReceiveTimeout = 200;
                    udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                    if (allowRouting)
                    {
                        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);
                    }

                    udpClient.Client.Bind(new IPEndPoint(bindAddress, 0));

                    while (true)
                    {
                        SendMessage(udpClient);
                        lastSend = DateTime.Now;

                        try
                        {
                            if (killTrigger.WaitOne(timeBetweenBroadcastsMs))
                            {
                                clientListen.Kill(true);
                                return;
                            }
                        }
                        catch (ThreadAbortException)
                        {
                            clientListen.Kill(true);
                            return;
                        }
                    }
                }
            }

            private void SendMessage(UdpClient client)
            {
                var requestData = Encoding.ASCII.GetBytes(JsonUtility.ToJson(new ClientRequest(listenPort)));

                client.Send(requestData, requestData.Length, new IPEndPoint(sendAddress, editorDiscoveryPort));
            }

            private ServerInfo[] GetServerInfos()
            {
                return clientListen.GetServerInfos();
            }

            public NetworkInterfaceInfo GetNetworkInterfaceInfo()
            {
                return new NetworkInterfaceInfo(
                    networkInterface.Name,
                    networkInterface.GetBindingAddress(),
                    networkInterface.GetSendAddress(),
                    GetServerInfos(),
                    lastSend);
            }
        }
    }
}
