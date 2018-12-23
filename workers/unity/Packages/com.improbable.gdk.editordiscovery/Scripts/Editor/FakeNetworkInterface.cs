using System.Net;
using System.Net.NetworkInformation;

namespace Improbable.GDK.EditorDiscovery
{
    internal class FakeNetworkInterface : INetworkInterface
    {
        private readonly IPAddress bindAddress;
        private readonly IPAddress sendAddress;

        public FakeNetworkInterface(string name, IPAddress bindAddress, IPAddress sendAddress)
        {
            Name = name;
            this.bindAddress = bindAddress;
            this.sendAddress = sendAddress;
        }

        public string Name { get; }

        public string Id => $"fake: {Name}";

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
