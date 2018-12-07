using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public interface IComponentDiffStorage
    {
        uint GetComponentId();

        void Clean();
    }

    public interface ICommandDiffStorage
    {
        uint GetComponentId();
        uint GetCommandId();

        void Clean();
    }

    public interface IDiffUpdateStorage<T> : IComponentDiffStorage
        where T : ISpatialComponentUpdate
    {
        void AddUpdate(ComponentUpdateReceived<T> update);
        void GetUpdates(long entityId, ICollection<ComponentUpdateReceived<T>> messageList);
    }

    public interface IDiffEventStorage<T> : IComponentDiffStorage
        where T : IEvent
    {
        void AddEvent(ComponentEventReceived<T> ev);
        void GetEvents(long entityId, ICollection<ComponentEventReceived<T>> messageList);
    }

    public interface IDiffCommandRequestStorage<T> : ICommandDiffStorage
        where T : IReceivedCommandRequest
    {
        void AddRequest(T request);
        void GetRequests(ICollection<T> messageList);
    }

    public interface IDiffCommandResponseStorage<T> : ICommandDiffStorage
        where T : IRawReceivedCommandResponse
    {
        void AddResponse(T response);
        void GetResponses(ICollection<T> messageList);
    }
}
