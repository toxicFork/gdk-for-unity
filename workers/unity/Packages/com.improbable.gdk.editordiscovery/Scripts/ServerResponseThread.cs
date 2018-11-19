using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

[Serializable]
public class EditorDiscoveryResponse
{
    public string serverName;
    public string companyName;
    public string productName;
    public string dataPath;
}

public class ServerResponseThread
{
    private readonly IPEndPoint remoteEp;
    private readonly EditorDiscoveryResponse serverInfo;

    private ServerResponseThread(EditorDiscoveryResponse serverInfo, IPEndPoint remoteEp)
    {
        this.serverInfo = serverInfo;
        this.remoteEp = remoteEp;
    }

    private void Start()
    {
        using (var sendClient = new UdpClient())
        {
            byte[] responseData = Encoding.ASCII.GetBytes(JsonUtility.ToJson(serverInfo));

            sendClient.Send(responseData, responseData.Length, remoteEp);
        }
    }

    public static void StartThread(EditorDiscoveryResponse serverInfo, IPEndPoint remoteEp)
    {
        new Thread(() =>
        {
            new ServerResponseThread(serverInfo, remoteEp).Start();
        }).Start();
    }
}
