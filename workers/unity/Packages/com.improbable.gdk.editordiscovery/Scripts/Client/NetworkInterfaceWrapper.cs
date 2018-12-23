using System;
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
        public string Id => networkInterface.Id;

        public OperationalStatus OperationalStatus => networkInterface.OperationalStatus;

        private IPAddress bindingAddressCache;

        public IPAddress GetBindingAddress()
        {
            if (bindingAddressCache == null && networkInterface.OperationalStatus != OperationalStatus.Down)
            {
                try
                {
                    bindingAddressCache = networkInterface
                        .GetIPProperties()
                        .UnicastAddresses
                        .FirstOrDefault(address => address.Address.AddressFamily == AddressFamily.InterNetwork)
                        ?.Address;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return bindingAddressCache;
        }

        public IPAddress GetSendAddress()
        {
            return IPAddress.Broadcast;
        }
    }
}
