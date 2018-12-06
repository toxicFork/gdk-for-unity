namespace Improbable.GDK.EditorDiscovery
{
    public class NetworkInterfaceInfo
    {
        public readonly string NetworkInterfaceName;
        public readonly ServerInfo[] ServerInfos;

        public NetworkInterfaceInfo(string networkInterfaceName, ServerInfo[] serverInfos)
        {
            NetworkInterfaceName = networkInterfaceName;
            ServerInfos = serverInfos;
        }
    }
}
