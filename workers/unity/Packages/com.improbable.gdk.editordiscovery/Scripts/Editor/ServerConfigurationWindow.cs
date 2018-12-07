using System;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal class ServerConfigurationWindow : EditorWindow
    {
        private EditorDiscoveryServer _editorDiscoveryServer;
        private ClientNetworkInterface _clientNetworkInterface;
        private EditorDiscoveryClient _editorDiscoveryClient;

        private string _serverName;

        private string _clientSendAddressString = "127.255.255.255";

        private int _editorDiscoveryPort = 8888;

        [MenuItem("SpatialOS/Editor Discovery Window")]
        private static void ShowWindow()
        {
            GetWindow<ServerConfigurationWindow>().Show();
        }

        public void OnEnable()
        {
            _serverName = EditorPrefs.GetString("discovery-server-name", "default");
        }

        // ReSharper disable once InvertIf
        public void OnDisable()
        {
            if (_editorDiscoveryServer != null)
            {
                _editorDiscoveryServer.Kill(true);
                _editorDiscoveryServer = null;
            }

            if (_clientNetworkInterface != null)
            {
                _clientNetworkInterface.Kill(true);
                _clientNetworkInterface = null;
            }

            if (_editorDiscoveryClient != null)
            {
                _editorDiscoveryClient.Kill(true);
                _editorDiscoveryClient = null;
            }
        }

        public void Update()
        {
            // This is necessary to make the framerate normal for the editor window.
            Repaint();
        }

        public void OnGUI()
        {
            using (new EditorGUI.DisabledScope(
                _editorDiscoveryClient != null ||
                _editorDiscoveryServer != null ||
                _clientNetworkInterface != null))
            {
                _editorDiscoveryPort = EditorGUILayout.IntField("Port", _editorDiscoveryPort);
            }

            EditorGUILayout.Space();

            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                _serverName = EditorGUILayout.TextField("Server Name", _serverName);

                if (changeCheck.changed)
                {
                    EditorPrefs.SetString("discovery-server-name", _serverName);

                    _editorDiscoveryServer?.SetName(_serverName);
                }
            }

            if (_editorDiscoveryServer == null)
            {
                if (GUILayout.Button("Start"))
                {
                    _editorDiscoveryServer = new EditorDiscoveryServer(_serverName, _editorDiscoveryPort, 10);
                    _editorDiscoveryServer.Start();
                }
            }
            else
            {
                if (GUILayout.Button("Stop"))
                {
                    _editorDiscoveryServer.Kill(true);
                    _editorDiscoveryServer = null;
                }
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Test client");
                GUILayout.FlexibleSpace();
            }

            using (new EditorGUI.DisabledScope(_clientNetworkInterface != null))
            {
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Local Broadcast"))
                    {
                        GUIUtility.hotControl = 0;
                        GUIUtility.keyboardControl = 0;
                        _clientSendAddressString = "127.255.255.255";
                    }

                    if (GUILayout.Button("Global Broadcast"))
                    {
                        GUIUtility.hotControl = 0;
                        GUIUtility.keyboardControl = 0;
                        _clientSendAddressString = IPAddress.Broadcast.ToString();
                        // clientListenAddressString = "127.0.0.1";
                    }
                }

                _clientSendAddressString = EditorGUILayout.TextField("Test address", _clientSendAddressString);
            }

            if (_clientNetworkInterface == null)
            {
                if (GUILayout.Button("Simple Client Start"))
                {
                    var sendAddress = IPAddress.Parse(_clientSendAddressString);

                    _clientNetworkInterface = new ClientNetworkInterface(
                        new FakeNetworkInterface("Hand-Crafted Network Interface", IPAddress.Any, sendAddress),
                        _editorDiscoveryPort,
                        1000,
                        20,
                        2000,
                        false);
                    _clientNetworkInterface.Start();
                }
            }
            else
            {
                if (GUILayout.Button("Client Stop"))
                {
                    _clientNetworkInterface.Kill(true);
                    _clientNetworkInterface = null;
                }
                else
                {
                    var networkInterfaceInfo = _clientNetworkInterface.GetNetworkInterfaceInfo();

                    DisplayNetworkInterfaceInfo(networkInterfaceInfo);
                }
            }

            if (_editorDiscoveryClient == null)
            {
                if (GUILayout.Button("Full Client Start"))
                {
                    _editorDiscoveryClient = new EditorDiscoveryClient(
                        _editorDiscoveryPort,
                        1000,
                        20,
                        5000,
                        5000,
                        new[]
                        {
                            new FakeLocalhostNetworkInterface()
                        }
                    );
                    _editorDiscoveryClient.Start();
                }
            }
            else
            {
                if (GUILayout.Button("Full Client Stop"))
                {
                    _editorDiscoveryClient.Kill(true);
                    _editorDiscoveryClient = null;
                }
                else
                {
                    foreach (var networkInterfaceInfo in _editorDiscoveryClient.GetNetworkInterfaceInfos())
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
                    EditorGUILayout.LabelField($"ServerName: {serverInfo.ServerResponse.ServerName}");

                    using (new EditorGUI.IndentLevelScope(1))
                    {
                        EditorGUILayout.LabelField($"DataPath: {serverInfo.ServerResponse.DataPath}");
                        EditorGUILayout.LabelField($"CompanyName: {serverInfo.ServerResponse.CompanyName}");
                        EditorGUILayout.LabelField($"ProductName: {serverInfo.ServerResponse.ProductName}");
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
