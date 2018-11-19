using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ServerListenThreadHandle
{
    private readonly ServerListenThread serverListenThread;
    private readonly Thread thread;

    private readonly ConcurrentQueue<string> serverNameQueue = new ConcurrentQueue<string>();
    private readonly ManualResetEvent killTrigger = new ManualResetEvent(false);

    public ServerListenThreadHandle(string serverName, int editorDiscoveryPort, int packetReceiveTimeoutMs)
    {
        serverListenThread = new ServerListenThread(serverName, editorDiscoveryPort, packetReceiveTimeoutMs,
            killTrigger, serverNameQueue,
            Application.dataPath, Application.companyName, Application.productName);

        thread = new Thread(serverListenThread.Start);
    }

    internal void Start()
    {
        thread.Start();
    }

    public void SetName(string newName)
    {
        serverNameQueue.Enqueue(newName);
    }

    public void Kill()
    {
        killTrigger.Set();
    }
}

public class ServerListenThread
{
    private readonly int packetReceiveTimeoutMs;
    private readonly int editorDiscoveryPort;
    private readonly ManualResetEvent killTrigger;
    private readonly ConcurrentQueue<string> serverNameQueue;

    private string serverName;

    private readonly string dataPath;
    private readonly string companyName;
    private readonly string productName;

    internal ServerListenThread(
        string serverName,
        int editorDiscoveryPort,
        int packetReceiveTimeoutMs,
        ManualResetEvent killTrigger,
        ConcurrentQueue<string> serverNameQueue,
        string dataPath,
        string companyName,
        string productName)
    {
        this.serverName = serverName;
        this.packetReceiveTimeoutMs = packetReceiveTimeoutMs;
        this.editorDiscoveryPort = editorDiscoveryPort;
        this.killTrigger = killTrigger;
        this.serverNameQueue = serverNameQueue;

        this.dataPath = dataPath;
        this.companyName = companyName;
        this.productName = productName;
    }

    internal void Start()
    {
        using (var client = new UdpClient())
        {
            var socket = client.Client;

            // Allows multiple server listen threads to listen on the same port
            // e.g. multiple Unity editor instances in the same computer.
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.Bind(new IPEndPoint(IPAddress.Any, editorDiscoveryPort));

            try
            {
                // main loop
                while (true)
                {
                    switch (TryReceive(client, packetReceiveTimeoutMs, killTrigger, out var remoteEp,
                        out var receivedBytes))
                    {
                        case ReceiveResult.Success:
                            Debug.Log(
                                $">>>>> Rec: {Encoding.ASCII.GetString(receivedBytes)} from {remoteEp.Address} {remoteEp.Port}");

                            while (!serverNameQueue.IsEmpty)
                            {
                                if (serverNameQueue.TryDequeue(out var newServerName))
                                {
                                    serverName = newServerName;
                                }
                            }

                            var serverInfo = new EditorDiscoveryResponse
                            {
                                serverName = serverName,
                                companyName = companyName,
                                productName = productName,
                                dataPath = dataPath,
                            };

                            ServerResponseThread.StartThread(serverInfo, remoteEp);

                            break;
                        case ReceiveResult.Killed:
                            Debug.Log("Killed?");
                            return;
                        case ReceiveResult.Failure:
                            Debug.Log("Failed?");
                            return;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    public static ServerListenThreadHandle StartThread(string serverName, int editorDiscoveryPort, int packetReceiveTimeoutMs)
    {
        var handle = new ServerListenThreadHandle(serverName, editorDiscoveryPort, packetReceiveTimeoutMs);

        handle.Start();

        return handle;
    }

    private enum ReceiveResult
    {
        Success,
        Killed,
        Failure,
    }

    private static ReceiveResult TryReceive(
        UdpClient client,
        int packetReceiveTimeoutMs,
        WaitHandle killTrigger,
        out IPEndPoint remoteEndPoint,
        out byte[] receivedBytes)
    {
        var beginReceive = client.BeginReceive(null, null);
        var handle = beginReceive.AsyncWaitHandle;

        remoteEndPoint = null;
        receivedBytes = null;

        while (true)
        {
            if (handle.WaitOne(packetReceiveTimeoutMs))
            {
                if (!beginReceive.IsCompleted)
                {
                    return ReceiveResult.Failure;
                }

                var remoteEp = new IPEndPoint(IPAddress.Any, 0);
                var result = client.EndReceive(beginReceive, ref remoteEp);

                remoteEndPoint = remoteEp;
                receivedBytes = result;

                return ReceiveResult.Success;
            }

            if (killTrigger.WaitOne(0))
            {
                return ReceiveResult.Killed;
            }
        }
    }
}
