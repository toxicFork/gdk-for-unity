using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientNetworkInterfaceThread
    {
        private readonly IPAddress bindAddress;
        private readonly IPAddress sendAddress;
        private readonly int editorDiscoveryPort;
        private readonly int timeBetweenBroadcastsMs;
        private readonly int packetReceiveTimeoutMs;

        private readonly int staleServerResponseTimeMs;

        private readonly bool allowRouting;
        private readonly ManualResetEvent killTrigger;

        private int listenPort;
        private readonly ClientListenThreadHandle clientListenThreadHandle;

        private readonly INetworkInterface networkInterface;

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
            this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
            this.staleServerResponseTimeMs = staleServerResponseTimeMs;
            this.allowRouting = allowRouting;
            this.killTrigger = killTrigger;

            clientListenThreadHandle = new ClientListenThreadHandle(
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs);
        }

        public void ThreadMethod()
        {
            clientListenThreadHandle.Start();
            clientListenThreadHandle.WaitForReady();
            listenPort = clientListenThreadHandle.GetPort();

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

                Debug.Log($"Client broadcasting via address: {bindAddress} to {sendAddress}");


                while (true)
                {
                    SendMessage(udpClient);

                    try
                    {
                        if (killTrigger.WaitOne(timeBetweenBroadcastsMs))
                        {
                            clientListenThreadHandle.Kill(true);
                            return;
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        clientListenThreadHandle.Kill(true);
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

        public ServerInfo[] GetServerInfos()
        {
            return clientListenThreadHandle.GetServerInfos();
        }

        public NetworkInterfaceInfo GetNetworkInterfaceInfo()
        {
            return new NetworkInterfaceInfo(networkInterface.Name, GetServerInfos());
        }
    }
}
