using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

[Serializable]
public class EditorDiscoveryResponse
{
    public string ServerName;
    public string CompanyName;
    public string ProductName;
    public string DataPath;
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
        try
        {
            using (var sendClient = new UdpClient())
            {
                var json = JsonUtility.ToJson(serverInfo);

                byte[] responseData = Encoding.ASCII.GetBytes(json);

                Debug.Log("Sending response message: " + json);
                sendClient.Send(responseData, responseData.Length, remoteEp);
                Debug.Log("Sent response message: " + json);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    internal static Thread StartThread(EditorDiscoveryResponse serverInfo, IPEndPoint remoteEp)
    {
        var thread = new Thread(() => { new ServerResponseThread(serverInfo, remoteEp).Start(); });

        thread.Start();

        return thread;
    }
}
