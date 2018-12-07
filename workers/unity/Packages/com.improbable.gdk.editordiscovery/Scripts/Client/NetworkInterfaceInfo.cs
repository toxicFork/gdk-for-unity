using System;
using System.Net;

namespace Improbable.GDK.EditorDiscovery
{
    public class NetworkInterfaceInfo
    {
        public readonly string NetworkInterfaceName;
        public readonly IPAddress BindAddress;
        public readonly IPAddress SendAddress;
        public readonly ServerInfo[] ServerInfos;
        public readonly DateTime LastSend;

        public NetworkInterfaceInfo(
            string networkInterfaceName,
            IPAddress bindAddress,
            IPAddress sendAddress,
            ServerInfo[] serverInfos,
            DateTime lastSend)
        {
            NetworkInterfaceName = networkInterfaceName;
            BindAddress = bindAddress;
            SendAddress = sendAddress;
            ServerInfos = serverInfos;
            LastSend = lastSend;
        }
    }
}
