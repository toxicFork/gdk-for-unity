using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core.Commands
{
    public interface IReceivedResponse
    {
        EntityId EntityId { get; }
        string Message { get; }
        StatusCode StatusCode { get; }
        long RequestId { get; }
    }
}


