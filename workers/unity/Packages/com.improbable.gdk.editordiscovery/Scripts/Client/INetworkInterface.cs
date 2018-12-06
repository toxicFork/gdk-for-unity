using System.Net;
using System.Net.NetworkInformation;

namespace Improbable.GDK.EditorDiscovery
{
    internal interface INetworkInterface
    {
        string Name { get; }

        OperationalStatus OperationalStatus { get; }

        IPAddress GetBindingAddress();

        IPAddress GetSendAddress();
    }
}
