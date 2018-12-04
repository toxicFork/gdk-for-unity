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
        private readonly IPAddress clientListenAddress;
        private readonly int editorDiscoveryPort;
        private readonly int timeBetweenBroadcastsMs;
        private readonly int packetReceiveTimeoutMs;
        private readonly int staleServerResponseTimeMs;
        private readonly bool isBroadcast;
        private readonly ManualResetEvent killTrigger;

        public ClientNetworkInterfaceThread(
            IPAddress sendAddress,
            IPAddress clientListenAddress,
            int editorDiscoveryPort,
            int timeBetweenBroadcastsMs,
            int packetReceiveTimeoutMs,
            int staleServerResponseTimeMs,
            bool isBroadcast,
            ManualResetEvent killTrigger)
        {
            this.sendAddress = sendAddress;
            this.clientListenAddress = clientListenAddress;
            this.editorDiscoveryPort = editorDiscoveryPort;
            this.timeBetweenBroadcastsMs = timeBetweenBroadcastsMs;
            this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
            this.staleServerResponseTimeMs = staleServerResponseTimeMs;
            this.isBroadcast = isBroadcast;
            this.killTrigger = killTrigger;
        }

        private static void WaitForListenPort(int clientPort, AutoResetEvent listenKillEvent)
        {
            var waitHandle = new AutoResetEvent(false);

            new Thread(() =>
            {
                using (var receiveClient = new UdpClient())
                {
                    receiveClient.Client.SetSocketOption(SocketOptionLevel.Socket,
                        SocketOptionName.ReuseAddress, true);
                    receiveClient.Client.Bind(new IPEndPoint(IPAddress.Any, clientPort));

                    Debug.Log($"Receiver for port {clientPort} ready.");

                    waitHandle.Set();

                    while (true)
                    {
                        var receiveResult = receiveClient.BeginReceive(null, null);
                        var receiveHandle = receiveResult.AsyncWaitHandle;

                        while (true)
                        {
                            if (receiveHandle.WaitOne(10))
                            {
                                var serverEp = new IPEndPoint(IPAddress.Any, 0);
                                var serverResponseData = receiveClient.EndReceive(receiveResult, ref serverEp);
                                var serverResponse = Encoding.ASCII.GetString(serverResponseData);
                                Debug.Log($"{clientPort} Received {serverResponse} from {serverEp.Address}");
                                // if (killEvent.WaitOne(0))
                                // {
                                //     Debug.Log($"Port {clientPort} can die now I guess");
                                //     return;
                                // }
                                break;
                            }
                            else
                            {
                                if (listenKillEvent.WaitOne(0))
                                {
                                    Debug.LogError($"{clientPort} Exit?!");

                                    return;
                                }
                            }
                        }
                    }
                }
            }).Start();

            // wait for receiver to be ready
            waitHandle.WaitOne();
        }

        public void Start()
        {
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

                var clientPort = ((IPEndPoint) udpClient.Client.LocalEndPoint).Port;

                Debug.Log($"Client sending to address: {sendAddress}");
                Debug.Log($"Client receiving from address: {clientListenAddress}");

                // NetworkInterface.GetAllNetworkInterfaces().First().GetIPProperties().GetIPv4Properties().
                var clientListenThreadHandle = new ClientListenThreadHandle(
                    new IPEndPoint(clientListenAddress, clientPort),
                    packetReceiveTimeoutMs,
                    staleServerResponseTimeMs);

                clientListenThreadHandle.Start();
                clientListenThreadHandle.WaitForReady();

                while (true)
                {
                    if (isBroadcast)
                    {
                        BroadcastMessage(udpClient);
                    }
                    else
                    {
                        var requestData = Encoding.ASCII.GetBytes($"SomeRequestData DIRECT {sendAddress}");

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
