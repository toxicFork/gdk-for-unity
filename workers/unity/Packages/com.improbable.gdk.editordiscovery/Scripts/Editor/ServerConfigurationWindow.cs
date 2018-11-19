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
                    serverListenThread.Kill();
                    serverListenThread = null;
                }
            }
        }
    }
}
