using System;
using System.Net;

namespace Improbable.GDK.EditorDiscovery
{
    public class ServerInfo
    {
        public readonly EditorDiscoveryResponse serverResponse;
        public readonly DateTime ResponseTime;
        public readonly IPAddress IPAddress;

        public ServerInfo(IPAddress ipAddress, EditorDiscoveryResponse editorDiscoveryResponse)
        {
            serverResponse = editorDiscoveryResponse;
            ResponseTime = DateTime.Now;
            IPAddress = ipAddress;
        }
    }
}