using System.Collections.Generic;
using System.Threading;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     A SpatialOS worker instance.
    /// </summary>
    [DisableAutoCreation]
    public class WorkerSystem : ComponentSystem
    {
        public bool Paused = false;
        public readonly Connection Connection;
        public readonly ILogDispatcher LogDispatcher;
        public readonly string WorkerType;
        public readonly Vector3 Origin;

        /// <summary>
        ///     An ECS entity that represents the Worker.
        /// </summary>
        public Entity WorkerEntity;

        internal readonly Dictionary<EntityId, Entity> EntityIdToEntity = new Dictionary<EntityId, Entity>();
        private AutoResetEvent resumedEvent;

        public WorkerSystem(Connection connection, ILogDispatcher logDispatcher, string workerType, Vector3 origin)
        {
            Connection = connection;
            LogDispatcher = logDispatcher;
            WorkerType = workerType;
            Origin = origin;
        }

        /// <summary>
        ///     Attempts to find an ECS entity associated with a SpatialOS entity ID.
        /// </summary>
        /// <param name="entityId">The SpatialOS entity ID.</param>
        /// <param name="entity">
        ///     When this method returns, contains the ECS entity associated with the SpatialOS entity ID if one was
        ///     found, else the default value for <see cref="Entity" />.
        /// </param>
        /// <returns>
        ///     True, if an ECS entity associated with the SpatialOS entity ID was found, false otherwise.
        /// </returns>
        public bool TryGetEntity(EntityId entityId, out Entity entity)
        {
            return EntityIdToEntity.TryGetValue(entityId, out entity);
        }

        /// <summary>
        ///     Checks whether a SpatialOS entity is checked out on this worker.
        /// </summary>
        /// <param name="entityId">The SpatialOS entity ID to check for.</param>
        /// <returns>True, if the SpatialOS entity is checked out on this worker, false otherwise.</returns>
        public bool HasEntity(EntityId entityId)
        {
            return EntityIdToEntity.ContainsKey(entityId);
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            var entityManager = World.GetOrCreateManager<EntityManager>();
            WorkerEntity = entityManager.CreateEntity(typeof(OnConnected), typeof(WorkerEntityTag));
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }

        public Queue<OpList> opListQueue = new Queue<OpList>();
        private Thread pokeThread;

        public void PokeConnectionFromThread()
        {
            resumedEvent = new AutoResetEvent(false);

            pokeThread = new Thread(() =>
            {
                while (true)
                {
                    Debug.Log($"Num ops enqueued: {opListQueue.Count}");
                    opListQueue.Enqueue(Connection.GetOpList(0));

                    if (resumedEvent.WaitOne(1000))
                    {
                        break;
                    }
                }
            });

            pokeThread.Start();
        }

        public void StopPoking()
        {
            resumedEvent.Set();

            pokeThread.Join();
        }
    }
}
