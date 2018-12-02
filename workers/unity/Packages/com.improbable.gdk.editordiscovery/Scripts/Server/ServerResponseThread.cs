using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ServerResponseThread
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

                    var responseData = Encoding.ASCII.GetBytes(json);

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

        internal static void StartThread(EditorDiscoveryResponse serverInfo, IPEndPoint remoteEp)
        {
            var thread = new Thread(() => { new ServerResponseThread(serverInfo, remoteEp).Start(); });

            thread.Start();
        }
    }
}
