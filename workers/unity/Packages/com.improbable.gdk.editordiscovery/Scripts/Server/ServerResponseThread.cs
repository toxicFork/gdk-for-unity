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
        private readonly IPEndPoint _remoteEp;
        private readonly EditorDiscoveryResponse _serverInfo;

        private ServerResponseThread(EditorDiscoveryResponse serverInfo, IPEndPoint remoteEp)
        {
            _serverInfo = serverInfo;
            _remoteEp = remoteEp;
        }

        private void ThreadMethod()
        {
            try
            {
                using (var sendClient = new UdpClient())
                {
                    var json = JsonUtility.ToJson(_serverInfo);

                    var responseData = Encoding.ASCII.GetBytes(json);

                    sendClient.Send(responseData, responseData.Length, _remoteEp);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        internal static void StartThread(EditorDiscoveryResponse serverInfo, IPEndPoint remoteEp)
        {
            var thread = new Thread(() => { new ServerResponseThread(serverInfo, remoteEp).ThreadMethod(); });

            thread.Start();
        }
    }
}
