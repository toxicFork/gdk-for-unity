using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Improbable.GDK.EditorDiscovery
{
    internal class NetworkInterfaceWrapper : INetworkInterface
    {
        private readonly NetworkInterface networkInterface;

        public NetworkInterfaceWrapper(NetworkInterface networkInterface)
        {
            this.networkInterface = networkInterface;
        }

        public string Name => networkInterface.Name;

        public OperationalStatus OperationalStatus => networkInterface.OperationalStatus;

        public IPAddress GetBindingAddress()
        {
            return networkInterface
                .GetIPProperties()
                .UnicastAddresses
                .FirstOrDefault(address => address.Address.AddressFamily == AddressFamily.InterNetwork)?.Address;
        }

        public IPAddress GetSendAddress()
        {
            return IPAddress.Broadcast;
        }
    }
}
