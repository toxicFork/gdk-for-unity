using System;
using System.Net;

namespace Improbable.GDK.EditorDiscovery
{
    public class ServerInfo
    {
        public readonly EditorDiscoveryResponse ServerResponse;
        public readonly DateTime ResponseTime;
        public readonly IPAddress IPAddress;

        public ServerInfo(IPAddress ipAddress, EditorDiscoveryResponse editorDiscoveryResponse)
        {
            ServerResponse = editorDiscoveryResponse;
            ResponseTime = DateTime.Now;
            IPAddress = ipAddress;
        }
    }
}
