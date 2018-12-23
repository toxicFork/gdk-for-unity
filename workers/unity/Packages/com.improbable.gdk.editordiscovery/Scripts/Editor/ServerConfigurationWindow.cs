using System;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ServerConfigurationWindow : EditorWindow
    {
        private EditorDiscoveryServer editorDiscoveryServer;
        private ClientNetworkInterface clientNetworkInterface;
        private EditorDiscoveryClient editorDiscoveryClient;

        private string serverName;

        private string clientSendAddressString = "127.255.255.255";

        private int editorDiscoveryPort = 8888;
        private NetworkInterfaceInfo singleNetworkInterfaceInfo;
        private NetworkInterfaceInfo[] fullClientNetworkInterfaceInfos;

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
            if (editorDiscoveryServer != null)
            {
                editorDiscoveryServer.Kill(true);
                editorDiscoveryServer = null;
            }

            if (clientNetworkInterface != null)
            {
                clientNetworkInterface.Kill(true);
                clientNetworkInterface = null;
            }

            if (editorDiscoveryClient != null)
            {
                editorDiscoveryClient.Kill(true);
                editorDiscoveryClient = null;
            }
        }

        public void Update()
        {
            singleNetworkInterfaceInfo = clientNetworkInterface?.GetNetworkInterfaceInfo();
            fullClientNetworkInterfaceInfos = editorDiscoveryClient?.GetNetworkInterfaceInfos();

            // This is necessary to make the framerate normal for the editor window.
            Repaint();
        }

        public void OnGUI()
        {
            using (new EditorGUI.DisabledScope(
                editorDiscoveryClient != null ||
                editorDiscoveryServer != null ||
                clientNetworkInterface != null))
            {
                editorDiscoveryPort = EditorGUILayout.IntField("Port", editorDiscoveryPort);
            }

            EditorGUILayout.Space();

            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                serverName = EditorGUILayout.TextField("Server Name", serverName);

                if (changeCheck.changed)
                {
                    EditorPrefs.SetString("discovery-server-name", serverName);

                    editorDiscoveryServer?.SetName(serverName);
                }
            }

            if (editorDiscoveryServer == null)
            {
                if (GUILayout.Button("Start"))
                {
                    editorDiscoveryServer = new EditorDiscoveryServer(serverName, editorDiscoveryPort, 10);
                    editorDiscoveryServer.Start();
                }
            }
            else
            {
                if (GUILayout.Button("Stop"))
                {
                    editorDiscoveryServer.Kill(true);
                    editorDiscoveryServer = null;
                }
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Test client");
                GUILayout.FlexibleSpace();
            }

            using (new EditorGUI.DisabledScope(clientNetworkInterface != null))
            {
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

                clientSendAddressString = EditorGUILayout.TextField("Test address", clientSendAddressString);
            }

            if (clientNetworkInterface == null)
            {
                if (GUILayout.Button("Simple Client Start"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        var sendAddress = IPAddress.Parse(clientSendAddressString);

                        clientNetworkInterface = new ClientNetworkInterface(
                            new FakeNetworkInterface("Hand-Crafted Network Interface", IPAddress.Any, sendAddress),
                            editorDiscoveryPort,
                            1000,
                            20,
                            2000,
                            false);
                        clientNetworkInterface.Start();
                    };
                }
            }
            else
            {
                if (GUILayout.Button("Client Stop"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        clientNetworkInterface.Kill(true);
                        clientNetworkInterface = null;
                    };
                }

                if (clientNetworkInterface != null && singleNetworkInterfaceInfo != null)
                {
                    DisplayNetworkInterfaceInfo(singleNetworkInterfaceInfo);
                }
            }

            if (editorDiscoveryClient == null)
            {
                if (GUILayout.Button("Full Client Start"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        editorDiscoveryClient = new EditorDiscoveryClient(
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
                        editorDiscoveryClient.Start();
                    };
                }
            }
            else
            {
                if (GUILayout.Button("Full Client Stop"))
                {
                    EditorApplication.delayCall += () =>
                    {
                        editorDiscoveryClient.Kill(true);
                        editorDiscoveryClient = null;
                    };
                }

                if (editorDiscoveryClient != null && fullClientNetworkInterfaceInfos != null)
                {
                    foreach (var networkInterfaceInfo in fullClientNetworkInterfaceInfos)
                    {
                        DisplayNetworkInterfaceInfo(networkInterfaceInfo);
                    }
                }
            }
        }

        private static void DisplayNetworkInterfaceInfo(NetworkInterfaceInfo networkInterfaceInfo)
        {
            EditorGUILayout.LabelField(
                $"{networkInterfaceInfo.NetworkInterfaceName} {networkInterfaceInfo.BindAddress} -> {networkInterfaceInfo.SendAddress}");

            var networkInterfaceLastSendTimeSpan = (DateTime.Now - networkInterfaceInfo.LastSend);

            EditorGUILayout.LabelField(
                $"Time since last send: {networkInterfaceLastSendTimeSpan.TotalSeconds + networkInterfaceLastSendTimeSpan.TotalMilliseconds * 0.001f:0.##}s");

            using (new EditorGUI.IndentLevelScope(1))
            {
                foreach (var serverInfo in networkInterfaceInfo.ServerInfos)
                {
                    EditorGUILayout.LabelField($"ServerName: {serverInfo.ServerResponse.serverName}");

                    using (new EditorGUI.IndentLevelScope(1))
                    {
                        EditorGUILayout.LabelField($"DataPath: {serverInfo.ServerResponse.dataPath}");
                        EditorGUILayout.LabelField($"CompanyName: {serverInfo.ServerResponse.companyName}");
                        EditorGUILayout.LabelField($"ProductName: {serverInfo.ServerResponse.productName}");
                        EditorGUILayout.LabelField($"IPAddress: {serverInfo.IPAddress}");
                        var timeSpan = (DateTime.Now - serverInfo.ResponseTime);
                        EditorGUILayout.LabelField(
                            $"Time since last response: {timeSpan.TotalSeconds + timeSpan.TotalMilliseconds * 0.001f:0.##}s");
                        EditorGUILayout.Space();
                    }
                }
            }
        }
    }
}
