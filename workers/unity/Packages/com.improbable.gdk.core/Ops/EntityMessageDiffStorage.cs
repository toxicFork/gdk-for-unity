using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    internal class EntityMessageDiffStorage<T> where T : IReceivedEntityMessage
    {
        private readonly ListPool<T> listPool = new ListPool<T>(10, 2, 200);

        private readonly Dictionary<long, List<T>> entityIdToMessages = new Dictionary<long, List<T>>();

        // used to return lists to the pool on clear - avoid iterating over the dictionary
        private readonly List<List<T>> pooledListsInUse = new List<List<T>>();

        public void Clean()
        {
            foreach (var pooledList in pooledListsInUse)
            {
                listPool.Return(pooledList);
            }

            pooledListsInUse.Clear();
            entityIdToMessages.Clear();
        }

        public void AddMessage(T message)
        {
            if (!entityIdToMessages.TryGetValue(message.GetEntityId().Id, out var events))
            {
                events = listPool.Rent();
                entityIdToMessages.Add(message.GetEntityId().Id, events);
                pooledListsInUse.Add(events);
            }

            events.Add(message);
        }

        public void GetMessages(long entityId, ICollection<T> messageList)
        {
            if (entityIdToMessages.TryGetValue(entityId, out var messages))
            {
                foreach (var message in messages)
                {
                    messageList.Add(message);
                }
            }
        }
    }
}
