using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ServerConfigurationWindow : EditorWindow
    {
        private ServerListenThreadHandle serverListenThread;
        private ClientNetworkInterfaceThreadHandle clientNetworkInterfaceThreadHandle;

        private string serverName;

        private string clientSendAddressString = "127.255.255.255";
        private string clientListenAddressString = "127.0.0.1";

        private int editorDiscoveryPort = 8888;

        [MenuItem("SpatialOS/Editor Discovery Window")]
        private static void ShowWindow()
        {
            GetWindow<ServerConfigurationWindow>().Show();
        }

        public void OnEnable()
        {
            serverName = EditorPrefs.GetString("discovery-server-name", "default");

            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                Debug.Log($"Network interface {networkInterface.Name}");

                foreach (var addressInformation in networkInterface.GetIPProperties().UnicastAddresses
                    .Where(info => info.Address.AddressFamily == AddressFamily.InterNetwork)
                )
                {
                    Debug.Log($"    Address: {addressInformation.Address.AddressFamily} {addressInformation.Address}");
                }

                foreach (var addressInformation in networkInterface.GetIPProperties().AnycastAddresses
                    .Where(info => info.Address.AddressFamily == AddressFamily.InterNetwork)
                )
                {
                    Debug.Log(
                        $"    Anycast Address: {addressInformation.Address.AddressFamily} {addressInformation.Address}");
                }

                foreach (var addressInformation in networkInterface.GetIPProperties().MulticastAddresses
                    .Where(info => info.Address.AddressFamily == AddressFamily.InterNetwork)
                )
                {
                    Debug.Log(
                        $"    Multicast Address: {addressInformation.Address.AddressFamily} {addressInformation.Address}");
                }

                foreach (var addressInformation in networkInterface.GetIPProperties().GatewayAddresses
                    .Where(info => info.Address.AddressFamily == AddressFamily.InterNetwork)
                )
                {
                    Debug.Log(
                        $"    Gateway Address: {addressInformation.Address.AddressFamily} {addressInformation.Address}");
                }
            }
        }

        // ReSharper disable once InvertIf
        public void OnDisable()
        {
            if (serverListenThread != null)
            {
                serverListenThread.Kill(true);
                serverListenThread = null;
            }

            if (clientNetworkInterfaceThreadHandle != null)
            {
                clientNetworkInterfaceThreadHandle.Kill(true);
                clientNetworkInterfaceThreadHandle = null;
            }
        }

        public void OnGUI()
        {
            editorDiscoveryPort = EditorGUILayout.IntField("Port", editorDiscoveryPort);

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

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
                    serverListenThread = ServerListenThread.StartThread(serverName, editorDiscoveryPort, 10);
                }
            }
            else
            {
                if (GUILayout.Button("Stop"))
                {
                    serverListenThread.Kill(true);
                    serverListenThread = null;
                }
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Test client");
                GUILayout.FlexibleSpace();
            }

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Local Broadcast"))
                {
                    GUIUtility.hotControl = 0;
                    GUIUtility.keyboardControl = 0;
                    clientSendAddressString = "127.255.255.255";
                    clientListenAddressString = "127.0.0.1";
                }

                if (GUILayout.Button("Global Broadcast"))
                {
                    GUIUtility.hotControl = 0;
                    GUIUtility.keyboardControl = 0;
                    clientSendAddressString = IPAddress.Broadcast.ToString();
                    // clientListenAddressString = "127.0.0.1";
                }
            }

            // var loopbackInterface = NetworkInterface.GetAllNetworkInterfaces()[NetworkInterface.LoopbackInterfaceIndex];
            // Debug.Log($"loopback: {loopbackInterface.Name} | {loopbackInterface.OperationalStatus} | {loopbackInterface.GetIPProperties().UnicastAddresses.First().Address}");

            // for (var index = 0; index < NetworkInterface.GetAllNetworkInterfaces().Length; index++)
            // {
            //     var networkInterface = NetworkInterface.GetAllNetworkInterfaces()[index];
            //     if (index != NetworkInterface.LoopbackInterfaceIndex && networkInterface.OperationalStatus == OperationalStatus.Down)
            //     {
            //         continue;
            //     }
            //
            //     foreach (var unicastAddress in networkInterface.GetIPProperties().UnicastAddresses
            //         .Where(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork))
            //     {
            //         if (GUILayout.Button($"{networkInterface.Name} via {unicastAddress.Address}"))
            //         {
            //             clientSendAddressString = "255.255.255.255";
            //             clientListenAddressString = unicastAddress.Address.ToString();
            //         }
            //     }
            // }

            clientSendAddressString = EditorGUILayout.TextField("Test address", clientSendAddressString);
            clientListenAddressString = EditorGUILayout.TextField("Client listen address", clientListenAddressString);

            if (clientNetworkInterfaceThreadHandle == null)
            {
                if (GUILayout.Button("Client Start"))
                {
                    var sendAddress = IPAddress.Parse(clientSendAddressString);
                    var clientListenAddress = IPAddress.Parse(clientListenAddressString);

                    clientNetworkInterfaceThreadHandle = new ClientNetworkInterfaceThreadHandle(
                        sendAddress,
                        clientListenAddress,
                        editorDiscoveryPort,
                        1000,
                        20,
                        2000,
                        false);
                    clientNetworkInterfaceThreadHandle.Start();
                }
            }
            else
            {
                if (GUILayout.Button("Client Stop"))
                {
                    clientNetworkInterfaceThreadHandle.Kill(true);
                    clientNetworkInterfaceThreadHandle = null;
                }
            }
        }
    }
}
