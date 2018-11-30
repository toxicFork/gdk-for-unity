using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEditor;

namespace UnityEngine
{
    public class ServerConfigurationWindow : EditorWindow
    {
        private ServerListenThreadHandle serverListenThread;
        private string serverName;

        [MenuItem("SpatialOS/Editor Discovery Window")]
        private static void ShowWindow()
        {
            GetWindow<ServerConfigurationWindow>().Show();
        }

        public void OnEnable()
        {
            serverName = EditorPrefs.GetString("discovery-server-name", "default");
        }

        public void OnDisable()
        {
            if (serverListenThread != null)
            {
                serverListenThread.Kill();
                serverListenThread = null;
            }
        }

        public void OnGUI()
        {
            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                serverName = EditorGUILayout.TextField("Server Name", serverName);

                if (changeCheck.changed)
                {
                    EditorPrefs.SetString("discovery-server-name", serverName);

                    serverListenThread?.SetName(serverName);
                }
            }

            if (serverListenThread == null)
            {
                if (GUILayout.Button("Start"))
                {
                    serverListenThread = ServerListenThread.StartThread(serverName, 8080, 10);
                }
            }
            else
            {
                if (GUILayout.Button("Stop"))
                {
                    serverListenThread.Kill(true);
                    serverListenThread = null;
                }

                if (GUILayout.Button("Test"))
                {
                    using (var client = new UdpClient())
                    {
                        var bytes = Encoding.ASCII.GetBytes("test string");

                        var testEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

                        client.Send(bytes, bytes.Length, testEndPoint);

                        var remoteEp = new IPEndPoint(IPAddress.Any, 0);
                        var receivedBytes = client.Receive(ref remoteEp);

                        Debug.Log("Received response: " + Encoding.ASCII.GetString(receivedBytes));
                    }
                }
            }
        }
    }
}
