using System.Net;
using System.Net.NetworkInformation;

namespace Improbable.GDK.EditorDiscovery
{
    internal class FakeLocalhostNetworkInterface : INetworkInterface
    {
        public string Name => "Fake Localhost Interface";

        public OperationalStatus OperationalStatus => OperationalStatus.Up;

        public IPAddress GetBindingAddress()
        {
            return IPAddress.Any;
        }

        public IPAddress GetSendAddress()
        {
            return IPAddress.Parse("127.255.255.255");
        }
    }
}
