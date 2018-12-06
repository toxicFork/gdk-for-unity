using System.Net;
using System.Net.NetworkInformation;

namespace Improbable.GDK.EditorDiscovery
{
    internal class FakeNetworkInterface : INetworkInterface
    {
        private readonly IPAddress bindAddress;
        private readonly IPAddress sendAddress;
        private readonly string name;

        public FakeNetworkInterface(string name, IPAddress bindAddress, IPAddress sendAddress)
        {
            this.name = name;
            this.bindAddress = bindAddress;
            this.sendAddress = sendAddress;
        }

        public string Name => name;
        public string Id => $"fake: {name}";
        public OperationalStatus OperationalStatus => OperationalStatus.Up;

        public IPAddress GetBindingAddress()
        {
            return bindAddress;
        }

        public IPAddress GetSendAddress()
        {
            return sendAddress;
        }
    }
}