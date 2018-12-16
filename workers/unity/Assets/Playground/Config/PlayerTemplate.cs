using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.QueryBasedInterest;
using Improbable.Gdk.TransformSynchronization;
using Interest = Improbable.Gdk.QueryBasedInterest.Interest;

namespace Playground
{
    public static class PlayerTemplate
    {
        public static EntityTemplate CreatePlayerEntityTemplate(string workerId, Improbable.Vector3f position)
        {
            var clientAttribute = $"workerId:{workerId}";

            var playerInput = PlayerInput.Component.CreateSchemaComponentData(0, 0, false);
            var launcher = Launcher.Component.CreateSchemaComponentData(100, 0);

            var score = Score.Component.CreateSchemaComponentData(0);
            var cubeSpawner = CubeSpawner.Component.CreateSchemaComponentData(new List<EntityId>());

            var safeZoneQuery = Interest.Query()
                .ComponentConstraint<Position.Component>()
                .Return<Position.Component, Metadata.Component, Score.Component>();
            var interest = new InterestTemplate()
                .AddQuery(Position.ComponentId, safeZoneQuery)
                .AddQuery(CubeSpawner.ComponentId, safeZoneQuery);
            
            var entityBuilder = EntityBuilder.Begin()
                .AddPosition(0, 0, 0, clientAttribute)
                .AddMetadata("Character", WorkerUtils.UnityGameLogic)
                .SetPersistence(false)
                .SetReadAcl(WorkerUtils.AllWorkerAttributes)
                .SetEntityAclComponentWriteAccess(WorkerUtils.UnityGameLogic)
                .AddComponent(playerInput, clientAttribute)
                .AddComponent(launcher, WorkerUtils.UnityGameLogic)
                .AddComponent(score, WorkerUtils.UnityGameLogic)
                .AddComponent(cubeSpawner, WorkerUtils.UnityGameLogic)
                .AddTransformSynchronizationComponents(clientAttribute)
                .AddPlayerLifecycleComponents(workerId, clientAttribute, WorkerUtils.UnityGameLogic)
                .AddComponent(interest, WorkerUtils.UnityGameLogic);

            return entityBuilder.Build();
        }
    }
}
