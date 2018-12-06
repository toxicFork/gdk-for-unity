using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ClientNetworkInterfaceThread
    {
        private readonly IPAddress sendAddress;
        private readonly int editorDiscoveryPort;
        private readonly int timeBetweenBroadcastsMs;
        private readonly int packetReceiveTimeoutMs;
        private readonly int staleServerResponseTimeMs;
        private readonly bool isBroadcast;
        private readonly ManualResetEvent killTrigger;

        public ClientNetworkInterfaceThread(
            IPAddress sendAddress,
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            bool isBroadcast,
            ManualResetEvent killTrigger)
        {
            this.sendAddress = sendAddress;
            this.editorDiscoveryPort = editorDiscoveryPort;
            this.timeBetweenBroadcastsMs = timeBetweenBroadcastsMs;
            this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
            this.staleServerResponseTimeMs = staleServerResponseTimeMs;
            this.isBroadcast = isBroadcast;
            this.killTrigger = killTrigger;
        }

        public void Start()
        {
            var clientListenThreadHandle = new ClientListenThreadHandle(
                packetReceiveTimeoutMs,
                staleServerResponseTimeMs);

            clientListenThreadHandle.Start();
            clientListenThreadHandle.WaitForReady();
            var listenPort = clientListenThreadHandle.GetPort();

            using (var udpClient = new UdpClient())
            {
                // udpClient.Client.SendTimeout = 200;
                // udpClient.Client.ReceiveTimeout = 200;
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                if (isBroadcast)
                {
                    udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);
                    udpClient.Client.Bind(new IPEndPoint(sendAddress, 0));
                }
                else
                {
                    udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
                }

                Debug.Log($"Client sending to address: {sendAddress}");

                while (true)
                {
                    if (isBroadcast)
                    {
                        BroadcastMessage(udpClient);
                    }
                    else
                    {
                        var requestData = Encoding.ASCII.GetBytes(JsonUtility.ToJson(new ClientRequest(listenPort)));

                        udpClient.Send(requestData, requestData.Length, new IPEndPoint(sendAddress, 8888));
                    }

                    if (killTrigger.WaitOne(timeBetweenBroadcastsMs))
                    {
                        clientListenThreadHandle.Kill(true);
                        return;
                    }
                }
            }
        }

        private void BroadcastMessage(UdpClient client)
        {
            var requestData = Encoding.ASCII.GetBytes("SomeRequestData");

            client.Send(requestData, requestData.Length, new IPEndPoint(IPAddress.Broadcast, editorDiscoveryPort));
        }
    }
}
