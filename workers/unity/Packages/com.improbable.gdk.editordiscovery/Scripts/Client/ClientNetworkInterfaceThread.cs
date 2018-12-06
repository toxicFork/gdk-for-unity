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
        private readonly IPAddress broadcastAddress;
        private readonly int editorDiscoveryPort;
        private readonly int timeBetweenBroadcastsMs;
        private readonly int packetReceiveTimeoutMs;

        private readonly int staleServerResponseTimeMs;

        private readonly bool allowRouting;
        private readonly ManualResetEvent killTrigger;

        private int listenPort;

        public ClientNetworkInterfaceThread(
            IPAddress bindAddress,
            IPAddress broadcastAddress,
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            bool allowRouting,
            ManualResetEvent killTrigger)
        {
            this.bindAddress = bindAddress;
            this.broadcastAddress = broadcastAddress;
            this.editorDiscoveryPort = editorDiscoveryPort;
            this.timeBetweenBroadcastsMs = timeBetweenBroadcastsMs;
            this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
            this.staleServerResponseTimeMs = staleServerResponseTimeMs;
            this.allowRouting = allowRouting;
            this.killTrigger = killTrigger;
        }

        public void Start()
        {
            var clientListenThreadHandle = new ClientListenThreadHandle(
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs);

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

                Debug.Log($"Client broadcasting via address: {bindAddress} to {broadcastAddress}");


                while (true)
                {
                    SendMessage(udpClient);

                    if (killTrigger.WaitOne(timeBetweenBroadcastsMs))
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

            client.Send(requestData, requestData.Length, new IPEndPoint(broadcastAddress, editorDiscoveryPort));
        }
    }
}
