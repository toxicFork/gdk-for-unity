using System;
using System.Linq;
using System.Net;
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
        private ClientInitThreadHandle clientInitThreadHandle;

        private string serverName;

        private string clientSendAddressString = "127.255.255.255";

        private int editorDiscoveryPort = 8888;

        [MenuItem("SpatialOS/Editor Discovery Window")]
        private static void ShowWindow()
        {
            GetWindow<ServerConfigurationWindow>().Show();
        }

        public void OnEnable()
        {
            serverName = EditorPrefs.GetString("discovery-server-name", "default");
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

            if (clientInitThreadHandle != null)
            {
                clientInitThreadHandle.Kill(true);
                clientInitThreadHandle = null;
            }
        }

        public void Update()
        {
            // This is necessary to make the framerate normal for the editor window.
            Repaint();
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
            //         .Where(addr => addr.Address.AddressFamily == AddressFamily.InaterNetwork))
            //     {
            //         if (GUILayout.Button($"{networkInterface.Name} via {unicastAddress.Address}"))
            //         {
            //             clientSendAddressString = "255.255.255.255";
            //             clientListenAddressString = unicastAddress.Address.ToString();
            //         }
            //     }
            // }

            clientSendAddressString = EditorGUILayout.TextField("Test address", clientSendAddressString);

            if (clientNetworkInterfaceThreadHandle == null)
            {
                if (GUILayout.Button("Simple Client Start"))
                {
                    var sendAddress = IPAddress.Parse(clientSendAddressString);

                    clientNetworkInterfaceThreadHandle = new ClientNetworkInterfaceThreadHandle(
                        new FakeNetworkInterface("custom", IPAddress.Any, sendAddress),
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

            if (clientInitThreadHandle == null)
            {
                if (GUILayout.Button("Full Client Start"))
                {
                    clientInitThreadHandle = new ClientInitThreadHandle(
                        editorDiscoveryPort,
                        1000,
                        20,
                        5000,
                        5000,
                        new[]
                        {
                            new FakeLocalhostNetworkInterface()
                        }
                    );
                    clientInitThreadHandle.Start();
                }
            }
            else
            {
                if (GUILayout.Button("Full Client Stop"))
                {
                    clientInitThreadHandle.Kill(true);
                    clientInitThreadHandle = null;
                }
                else
                {
                    foreach (var networkInterfaceInfo in clientInitThreadHandle.GetNetworkInterfaceInfos())
                    {
                        EditorGUILayout.LabelField(networkInterfaceInfo.NetworkInterfaceName);
                        using (new EditorGUI.IndentLevelScope(1))
                        {
                            foreach (var serverInfo in networkInterfaceInfo.ServerInfos)
                            {
                                EditorGUILayout.LabelField($"ServerName: {serverInfo.serverResponse.ServerName}");
                                using (new EditorGUI.IndentLevelScope(1))
                                {
                                    EditorGUILayout.LabelField($"DataPath: {serverInfo.serverResponse.DataPath}");
                                    EditorGUILayout.LabelField($"CompanyName: {serverInfo.serverResponse.CompanyName}");
                                    EditorGUILayout.LabelField($"ProductName: {serverInfo.serverResponse.ProductName}");
                                    EditorGUILayout.LabelField($"IPAddress: {serverInfo.IPAddress}");
                                    var timeSpan = (DateTime.Now - serverInfo.ResponseTime);
                                    EditorGUILayout.LabelField(
                                        $"Time: {timeSpan.TotalSeconds + timeSpan.TotalMilliseconds * 0.001f:0.##}");
                                    EditorGUILayout.Space();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
