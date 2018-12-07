using System.Net;
using System.Net.NetworkInformation;

namespace Improbable.GDK.EditorDiscovery
{
    internal class FakeNetworkInterface : INetworkInterface
    {
        private readonly IPAddress _bindAddress;
        private readonly IPAddress _sendAddress;

        public FakeNetworkInterface(string name, IPAddress bindAddress, IPAddress sendAddress)
        {
            Name = name;
            _bindAddress = bindAddress;
            _sendAddress = sendAddress;
        }

        public string Name { get; }

        public string Id => $"fake: {Name}";

        public OperationalStatus OperationalStatus => OperationalStatus.Up;

        public IPAddress GetBindingAddress()
        {
            return _bindAddress;
        }

        public IPAddress GetSendAddress()
        {
            return _sendAddress;
        }
    }
}
