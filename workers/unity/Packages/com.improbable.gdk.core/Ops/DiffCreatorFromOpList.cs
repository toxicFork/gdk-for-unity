using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    // todo need a dictionary pool for entitiyId based things
    // or use hashsets / lists of touples for (entityId, index) for add component
    internal class DiffCreatorFromOpList
    {
        public bool Disconnected { get; private set; }
        public bool Complete { get; private set; }

        private readonly ListPool<uint> listPool = new ListPool<uint>(50, 10, 5);

        //private readonly Dictionary<long, List<uint>> entitiesAdded = new Dictionary<long, List<uint>>();
        private readonly HashSet<long> entitiesAdded = new HashSet<long>();
        private readonly HashSet<long> entitiesRemoved = new HashSet<long>();

        // contains the index of the op the last op that adds the component
        private readonly Dictionary<uint, Dictionary<long, int>> componentsAddedIndex =
            new Dictionary<uint, Dictionary<long, int>>();

        private readonly Dictionary<uint, HashSet<long>> componentsRemoved = new Dictionary<uint, HashSet<long>>();
        private readonly Dictionary<uint, HashSet<long>> authorityGained = new Dictionary<uint, HashSet<long>>();
        private readonly Dictionary<uint, HashSet<long>> authorityLost = new Dictionary<uint, HashSet<long>>();
        private readonly Dictionary<uint, HashSet<long>> authorityLossImm = new Dictionary<uint, HashSet<long>>();
        private readonly Dictionary<uint, HashSet<long>> authorityLostTemp = new Dictionary<uint, HashSet<long>>();

        // todo make this a set of tuples? - makes it hard to drop them all when the component is removed
        // especially when there are lot of entities
        // could reduce one index still - first index could be entityId, although component Ids are the ones that will always be there
        // probably better to be expensive there than in the majority case although consistency is good - makes takeoffs easier to reason about
        private readonly Dictionary<uint, Dictionary<long, List<uint>>> componentUpdateIndices =
            new Dictionary<uint, Dictionary<long, List<uint>>>();

        // component Id to entityId to op indices
        private readonly Dictionary<uint, Dictionary<long, List<uint>>> commandRequestIndices =
            new Dictionary<uint, Dictionary<long, List<uint>>>();

        private readonly Dictionary<uint, IComponentDiffDeserializer> componentIdToComponentDeserializer =
            new Dictionary<uint, IComponentDiffDeserializer>();

        private readonly Dictionary<(uint, uint), ICommandDiffDeserializer> commandIdsToCommandDeserializer =
            new Dictionary<(uint, uint), ICommandDiffDeserializer>();

        private uint componentUpdateId;

        public DiffCreatorFromOpList()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IComponentDiffDeserializer).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (IComponentDiffDeserializer) Activator.CreateInstance(type);

                        componentIdToComponentDeserializer.Add(instance.GetComponentId(), instance);
                    }

                    if (typeof(ICommandDiffDeserializer).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (ICommandDiffDeserializer) Activator.CreateInstance(type);

                        commandIdsToCommandDeserializer.Add((instance.GetComponentId(), instance.GetCommandId()), instance);
                    }
                }
            }
        }

        public void ParseOpListIntoDiff(OpList opList, ViewDiff viewDiff)
        {
            componentUpdateId = 1;
            FindNetChanges(opList);
            ApplyRelevantOpsToDiff(opList, viewDiff);
        }

        public void Clean()
        {
            // componentsAdded
            // componentsRemoved
            // authorityGained
            // authorityLost
            // authorityLossImm
            // authorityLostTemp
            // componentUpdateIndices
            // commandRequestIndices
            // commandResponseIndices
        }

        private void FindNetChanges(OpList opList)
        {
            for (int i = 0; i < opList.GetOpCount(); ++i)
            {
                switch (opList.GetOpType(i))
                {
                    case OpType.Disconnect:
                        Disconnected = true;
                        return;
                    case OpType.FlagUpdate:
                        var flagOp = opList.GetFlagUpdateOp(i);
                        break;
                    case OpType.LogMessage:
                        var logOp = opList.GetLogMessageOp(i);
                        break;
                    case OpType.Metrics:
                        var metricsOp = opList.GetMetricsOp(i);
                        break;
                    case OpType.CriticalSection:
                        var criticalSectionOp = opList.GetCriticalSectionOp(i);
                        Complete = criticalSectionOp.InCriticalSection;
                        break;
                    case OpType.AddEntity:
                        var addEntityOp = opList.GetAddEntityOp(i);
                        HandleAddEntity(addEntityOp.EntityId);
                        break;
                    case OpType.RemoveEntity:
                        var removeEntityOp = opList.GetRemoveEntityOp(i);
                        HandleRemoveEntity(removeEntityOp.EntityId);
                        break;
                    case OpType.ReserveEntityIdResponse:
                        throw new InvalidOperationException("Not supposed to get this one");
                        break;
                    case OpType.ReserveEntityIdsResponse:
                        var reserveEntityIdsOp = opList.GetReserveEntityIdsResponseOp(i);
                        break;
                    case OpType.CreateEntityResponse:
                        var createEntityOp = opList.GetCreateEntityResponseOp(i);
                        break;
                    case OpType.DeleteEntityResponse:
                        var deleteEntityOp = opList.GetDeleteEntityResponseOp(i);
                        break;
                    case OpType.EntityQueryResponse:
                        var entityQueryOp = opList.GetEntityQueryResponseOp(i);
                        break;
                    case OpType.AddComponent:
                        var addComponentOp = opList.GetAddComponentOp(i);
                        HandleAddComponent(addComponentOp.EntityId, addComponentOp.Data.ComponentId, i);
                        break;
                    case OpType.RemoveComponent:
                        var removeComponentOp = opList.GetRemoveComponentOp(i);
                        HandleRemoveComponent(removeComponentOp.EntityId, removeComponentOp.ComponentId);
                        break;
                    case OpType.AuthorityChange:
                        var authorityChangeOp = opList.GetAuthorityChangeOp(i);
                        switch (authorityChangeOp.Authority)
                        {
                            case Authority.Authoritative:
                                HandleAuthorityGained(authorityChangeOp.EntityId, authorityChangeOp.ComponentId);
                                break;
                            case Authority.NotAuthoritative:
                                HandleAuthorityLost(authorityChangeOp.EntityId, authorityChangeOp.ComponentId);
                                break;
                            case Authority.AuthorityLossImminent:
                                HandleAuthorityLossImminent(authorityChangeOp.EntityId, authorityChangeOp.ComponentId);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case OpType.ComponentUpdate:
                        var componentUpdateOp = opList.GetComponentUpdateOp(i);
                        HandleComponentUpdate(componentUpdateOp.EntityId, componentUpdateOp.Update.ComponentId, (uint) i);
                        break;
                    case OpType.CommandRequest:
                        var commandRequestOp = opList.GetCommandRequestOp(i);
                        HandleCommandRequestReceived(commandRequestOp.EntityId, commandRequestOp.Request.ComponentId,
                            (uint) i);
                        break;
                    case OpType.CommandResponse:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ApplyRelevantOpsToDiff(OpList opList, ViewDiff viewDiff)
        {
            for (int i = 0; i < opList.GetOpCount(); ++i)
            {
                switch (opList.GetOpType(i))
                {
                    case OpType.Disconnect:
                        viewDiff.Disconnect(opList.GetDisconnectOp(i).Reason);
                        return;
                    case OpType.FlagUpdate:
                        var flagOp = opList.GetFlagUpdateOp(i);
                        break;
                    case OpType.LogMessage:
                        var logOp = opList.GetLogMessageOp(i);
                        break;
                    case OpType.Metrics:
                        var metricsOp = opList.GetMetricsOp(i);
                        break;
                    case OpType.CriticalSection:
                        Complete = opList.GetCriticalSectionOp(i).InCriticalSection;
                        break;
                    case OpType.AddEntity:
                        TryApplyAddEntity(opList.GetAddEntityOp(i), viewDiff);
                        break;
                    case OpType.RemoveEntity:
                        TryApplyRemoveEntity(opList.GetRemoveEntityOp(i), viewDiff);
                        break;
                    case OpType.ReserveEntityIdResponse:
                        throw new InvalidOperationException("Not supposed to get this one");
                        break;
                    case OpType.ReserveEntityIdsResponse:
                        var reserveEntityIdsOp = opList.GetReserveEntityIdsResponseOp(i);
                        break;
                    case OpType.CreateEntityResponse:
                        var createEntityOp = opList.GetCreateEntityResponseOp(i);
                        break;
                    case OpType.DeleteEntityResponse:
                        var deleteEntityOp = opList.GetDeleteEntityResponseOp(i);
                        break;
                    case OpType.EntityQueryResponse:
                        var entityQueryOp = opList.GetEntityQueryResponseOp(i);
                        break;
                    case OpType.AddComponent:
                        TryApplyAddComponent(opList.GetAddComponentOp(i), viewDiff, i);
                        break;
                    case OpType.RemoveComponent:
                        TryApplyRemoveComponent(opList.GetRemoveComponentOp(i), viewDiff);
                        break;
                    case OpType.AuthorityChange:
                        TryApplyAuthorityChangeOp(opList.GetAuthorityChangeOp(i), viewDiff);
                        break;
                    case OpType.ComponentUpdate:
                        TryApplyComponentUpdate(opList.GetComponentUpdateOp(i), viewDiff, i);
                        break;
                    case OpType.CommandRequest:
                        TryApplyCommandRequestReceived(opList.GetCommandRequestOp(i), viewDiff, i);
                        break;
                    case OpType.CommandResponse:
                        TryApplyCommandResponseReceived(opList.GetCommandResponseOp(i), viewDiff);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void HandleAddEntity(long entityId)
        {
            if (!entitiesRemoved.Remove(entityId))
            {
                entitiesAdded.Add(entityId);
            }
        }

        private void HandleRemoveEntity(long entityId)
        {
            if (!entitiesAdded.Remove(entityId))
            {
                entitiesRemoved.Add(entityId);
            }
        }

        private void HandleAddComponent(long entityId, uint componentId, int dataIndex)
        {
            if (!ClearComponentEntity(componentsRemoved, componentId, entityId))
            {
                var added = GetOrCreate(componentsAddedIndex, componentId);
                added.Add(entityId, dataIndex);
            }
        }

        private void HandleRemoveComponent(long entityId, uint componentId)
        {
            // Clear component updates that no longer apply
            ClearComponentEntityList(componentUpdateIndices, componentId, entityId);

            // If this has been added this tick then clear that
            bool addedThisFrame = false;
            if (componentsAddedIndex.TryGetValue(componentId, out var entityIdToIndex))
            {
                addedThisFrame = entityIdToIndex.Remove(entityId);
            }

            // If not added this frame then mark this component to be removed
            if (!addedThisFrame)
            {
                SetComponentEntity(componentsRemoved, componentId, entityId);
            }
        }

        private void HandleComponentUpdate(long entityId, uint componentId, uint opIndex)
        {
            GetOrCreateComponentEntityList(componentUpdateIndices, componentId, entityId).Add(opIndex);
        }

        private void HandleAuthorityGained(long entityId, uint componentId)
        {
            if (ClearComponentEntity(authorityLost, componentId, entityId))
            {
                SetComponentEntity(authorityLostTemp, componentId, entityId);
            }
            else
            {
                SetComponentEntity(authorityGained, componentId, entityId);
            }
        }

        private void HandleAuthorityLost(long entityId, uint componentId)
        {
            ClearComponentEntity(authorityLossImm, componentId, entityId);
            ClearComponentEntity(authorityLostTemp, componentId, entityId);

            if (!ClearComponentEntity(authorityGained, componentId, entityId))
            {
                SetComponentEntity(authorityLost, componentId, entityId);
            }

            // Clear command requests that can not be responded to
            ClearComponentEntityList(commandRequestIndices, componentId, entityId);
        }

        private void HandleAuthorityLossImminent(long entityId, uint componentId)
        {
            // Want to still get that authority was gained if that happened this frame
            SetComponentEntity(authorityLossImm, componentId, entityId);
        }

        private void HandleCommandRequestReceived(long entityId, uint componentId, uint opIndex)
        {
            GetOrCreateComponentEntityList(commandRequestIndices, componentId, entityId).Add(opIndex);
        }

        private void TryApplyAddEntity(AddEntityOp op, ViewDiff viewDiff)
        {
            if (entitiesAdded.Remove(op.EntityId))
            {
                viewDiff.AddEntity(op.EntityId);
            }
        }

        private void TryApplyRemoveEntity(RemoveEntityOp op, ViewDiff viewDiff)
        {
            if (entitiesRemoved.Remove(op.EntityId))
            {
                viewDiff.AddEntity(op.EntityId);
            }
        }

        private void TryApplyAddComponent(AddComponentOp op, ViewDiff viewDiff, int opIndex)
        {
            if (!componentsAddedIndex.TryGetValue(op.Data.ComponentId, out var entityIdToIndex))
            {
                return;
            }

            if (!entityIdToIndex.TryGetValue(op.EntityId, out var index))
            {
                return;
            }

            if (index != opIndex)
            {
                return;
            }

            entityIdToIndex.Remove(op.EntityId);

            if (!componentIdToComponentDeserializer.TryGetValue(op.Data.ComponentId, out var deserializer))
            {
                throw new ArgumentException("Component ID not recognised");
            }

            // Deserialize and apply to diff
            deserializer.AddComponent(op, viewDiff);
        }

        private void TryApplyRemoveComponent(RemoveComponentOp op, ViewDiff viewDiff)
        {
            if (ClearComponentEntity(componentsRemoved, op.ComponentId, op.EntityId))
            {
                viewDiff.RemoveComponent(op.EntityId, op.ComponentId);
            }
        }

        private void TryApplyComponentUpdate(ComponentUpdateOp op, ViewDiff viewDiff, int opIndex)
        {
            if (!componentUpdateIndices.TryGetValue(op.Update.ComponentId, out var entityIdToIndices))
            {
                return;
            }

            if (!entityIdToIndices.TryGetValue(op.EntityId, out var indices))
            {
                return;
            }

            if (!indices.Remove((uint) opIndex))
            {
                return;
            }

            if (indices.Count == 0)
            {
                entityIdToIndices.Remove(op.EntityId);
            }

            if (!componentIdToComponentDeserializer.TryGetValue(op.Update.ComponentId, out var deserializer))
            {
                throw new ArgumentException("Component ID not recognised");
            }

            // Deserialize and apply to diff
            deserializer.AddUpdate(op, viewDiff, componentUpdateId);
            ++componentUpdateId;
        }

        private void TryApplyAuthorityChangeOp(AuthorityChangeOp op, ViewDiff viewDiff)
        {
            switch (op.Authority)
            {
                case Authority.NotAuthoritative:
                    if (ClearComponentEntity(authorityLost, op.ComponentId, op.EntityId))
                    {
                        viewDiff.SetAuthority(op.EntityId, op.ComponentId, Authority.NotAuthoritative);
                    }

                    break;
                case Authority.Authoritative:
                    if (ClearComponentEntity(authorityGained, op.ComponentId, op.EntityId))
                    {
                        viewDiff.SetAuthority(op.EntityId, op.ComponentId, Authority.Authoritative);
                    }

                    break;
                case Authority.AuthorityLossImminent:
                    if (ClearComponentEntity(authorityLossImm, op.ComponentId, op.EntityId))
                    {
                        viewDiff.SetAuthority(op.EntityId, op.ComponentId, Authority.AuthorityLossImminent);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (ClearComponentEntity(authorityLostTemp, op.ComponentId, op.EntityId))
            {
                viewDiff.MarkAuthorityTemporarilyLost(op.EntityId, op.ComponentId);
            }
        }

        private void TryApplyCommandRequestReceived(CommandRequestOp op, ViewDiff viewDiff, int opIndex)
        {
            if (!commandRequestIndices.TryGetValue(op.Request.ComponentId, out var entityIdToIndices))
            {
                return;
            }

            if (!entityIdToIndices.TryGetValue(op.EntityId, out var indices))
            {
                return;
            }

            if (!indices.Remove((uint) opIndex))
            {
                return;
            }

            if (indices.Count == 0)
            {
                entityIdToIndices.Remove(op.EntityId);
            }

            if (!commandIdsToCommandDeserializer.TryGetValue((op.Request.ComponentId, op.Request.CommandIndex),
                out var deserializer))
            {
                throw new ArgumentException("Component ID not recognised");
            }

            // Deserialize and apply to diff
            deserializer.AddRequest(op, viewDiff);
        }

        private void TryApplyCommandResponseReceived(CommandResponseOp op, ViewDiff viewDiff)
        {
            if (!commandIdsToCommandDeserializer.TryGetValue((op.Response.ComponentId, op.Response.CommandIndex),
                out var deserializer))
            {
                throw new ArgumentException("Component ID not recognised");
            }

            // Deserialize and apply to diff
            deserializer.AddResponse(op, viewDiff);
        }

        private TValue GetOrCreate<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            if (!dict.TryGetValue(key, out var value))
            {
                value = new TValue();
                dict.Add(key, value);
            }

            return value;
        }

        private void SetComponentEntity(Dictionary<uint, HashSet<long>> dict, uint componentId,
            long entityId)
        {
            var set = GetOrCreate(dict, componentId);
            set.Add(entityId);
        }

        private bool ClearComponentEntity(Dictionary<uint, HashSet<long>> dict, uint componentId,
            long entityId)
        {
            if (!dict.TryGetValue(componentId, out var set))
            {
                return false;
            }

            return set.Remove(entityId);
        }

        private bool ComponentEntityValueIsSet(Dictionary<uint, HashSet<long>> dict, uint componentId,
            long entityId)
        {
            if (!dict.TryGetValue(componentId, out var set))
            {
                return false;
            }

            return set.Contains(entityId);
        }

        private bool ClearComponentEntityList(Dictionary<uint, Dictionary<long, List<uint>>> dict, uint componentId,
            long entityId)
        {
            if (!dict.TryGetValue(componentId, out var idToList))
            {
                return false;
            }

            if (!idToList.TryGetValue(entityId, out var list))
            {
                return false;
            }

            listPool.Return(list);
            idToList.Remove(entityId);
            return true;
        }

        private List<uint> GetOrCreateComponentEntityList(Dictionary<uint, Dictionary<long, List<uint>>> dict, uint componentId,
            long entityId)
        {
            if (!dict.TryGetValue(componentId, out var idToList))
            {
                idToList = new Dictionary<long, List<uint>>();
                dict.Add(componentId, idToList);
            }

            if (!idToList.TryGetValue(entityId, out var list))
            {
                list = listPool.Rent();
                idToList.Add(entityId, list);
            }

            return list;
        }
    }
}
