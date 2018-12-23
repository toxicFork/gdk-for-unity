using System.Net;
using System.Net.NetworkInformation;

namespace Improbable.GDK.EditorDiscovery
{
    public interface INetworkInterface
    {
        string Name { get; }
        string Id { get; }

        OperationalStatus OperationalStatus { get; }

        IPAddress GetBindingAddress();

        IPAddress GetSendAddress();
    }
}
