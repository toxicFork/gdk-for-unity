using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class ViewDiff
    {
        public bool Disconnected { get; private set; }
        public bool Complete { get; set; }

        private readonly List<EntityOp> entityOpList = new List<EntityOp>(50);

        private readonly ListPool<long> componentListPool = new ListPool<long>(50, 10, 50);
        private readonly ListPool<long> entityListPool = new ListPool<long>(50, 10, 50);

        private readonly Dictionary<uint, List<long>> componentIdToEntitiesUpdated = new Dictionary<uint, List<long>>();

        private readonly List<LogMessageOp> logs = new List<LogMessageOp>();
        private readonly List<MetricsOp> metrics = new List<MetricsOp>();
        private readonly List<FlagUpdateOp> flags = new List<FlagUpdateOp>();

        private readonly Dictionary<long, List<uint>> entityIdToComponentsAdded = new Dictionary<long, List<uint>>();
        private readonly Dictionary<long, List<uint>> entityIdToComponentsRemoved = new Dictionary<long, List<uint>>();

        private readonly Dictionary<uint, IComponentDiffStorage> componentIdToComponentStorage =
            new Dictionary<uint, IComponentDiffStorage>();

        private readonly List<IComponentDiffStorage> componentStorageList = new List<IComponentDiffStorage>();

        private readonly Dictionary<(uint, uint), ICommandDiffStorage> commandIdsToCommandStorage =
            new Dictionary<(uint, uint), ICommandDiffStorage>();

        private readonly List<ICommandDiffStorage> commandStorageList = new List<ICommandDiffStorage>();

        private string disconnectMessage;

        public ViewDiff()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IComponentDiffStorage).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (IComponentDiffStorage) Activator.CreateInstance(type);

                        componentStorageList.Add(instance);
                        componentIdToComponentStorage.Add(instance.GetComponentId(), instance);
                    }

                    if (typeof(ICommandDiffStorage).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (ICommandDiffStorage) Activator.CreateInstance(type);

                        commandStorageList.Add(instance);
                        commandIdsToCommandStorage.Add((instance.GetComponentId(), instance.GetCommandId()), instance);
                    }
                }
            }
        }

        public void Clean()
        {
            foreach (var storage in componentStorageList)
            {
                storage.Clean();
            }

            foreach (var storage in commandStorageList)
            {
                storage.Clean();
            }
        }

        public void AddEntity(long entityId)
        {
        }

        public void RemoveEntity(long entityId)
        {
            //Debug.Log("remove entity " + entityId);
        }

        // todo review is this can be something other than an update
        // see if it's possible to enforce that all fields are set
        public void AddComponent<T>(T component, long entityId, uint componentId) where T : ISpatialComponentUpdate
        {
            //Debug.Log("add component");
            // todo the actual add component to entity part of this. This is just the set initial value part
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException("Unknown component ID");
            }

            ((IDiffUpdateStorage<T>) storage).AddUpdate(new ComponentUpdateReceived<T>(component, new EntityId(entityId),
                0));
        }

        public void RemoveComponent(long entityId, uint componentId)
        {
            //Debug.Log("remove component " + entityId + " " + componentId);
        }

        public void SetAuthority(long entityId, uint componentId, Authority authority)
        {
            //Debug.Log("auth " + authority + " " + entityId + " " + componentId);
        }

        public void MarkAuthorityTemporarilyLost(long entityId, uint componentId)
        {
            //Debug.Log("temp auth");
        }

        public void AddComponentUpdate<T>(T update, long entityId, uint componentId, uint updateId)
            where T : ISpatialComponentUpdate
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException("Unknown component");
            }

            ((IDiffUpdateStorage<T>) storage).AddUpdate(new ComponentUpdateReceived<T>(update, new EntityId(entityId),
                updateId));
        }

        public void AddEvent<T>(T ev, long entityId, uint componentId, uint updateId) where T : IEvent
        {
            if (!componentIdToComponentStorage.TryGetValue(componentId, out var storage))
            {
                throw new ArgumentException("Unknown component");
            }

            ((IDiffEventStorage<T>) storage).AddEvent(new ComponentEventReceived<T>(ev, new EntityId(entityId),
                updateId));
        }

        public void AddCommandRequest<T>(T request, uint componentId, uint commandId) where T : IReceivedCommandRequest
        {
            //Debug.Log("request");
            if (!commandIdsToCommandStorage.TryGetValue((componentId, commandId), out var storage))
            {
                throw new ArgumentException("Unknown Command");
            }

            ((IDiffCommandRequestStorage<T>) storage).AddRequest(request);
        }

        public void AddCommandResponse<T>(T response, uint componentId, uint commandId) where T : IRawReceivedCommandResponse
        {
            //Debug.Log("response");
            if (!commandIdsToCommandStorage.TryGetValue((componentId, commandId), out var storage))
            {
                throw new ArgumentException("Unknown component ID");
            }

            ((IDiffCommandResponseStorage<T>) storage).AddResponse(response);
        }

        public void Disconnect(string message)
        {
            disconnectMessage = message;
        }
    }
}
