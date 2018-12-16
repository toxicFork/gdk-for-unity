using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.QueryBasedInterest;
using Improbable.Gdk.TransformSynchronization;
using UnityEngine;
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
                .AddPlayerLifecycleComponents(workerId, clientAttribute, WorkerUtils.UnityGameLogic);

            return entityBuilder.Build();
        }

        private static void MinimapExample()
        {
            var playerConstraint = Interest.Query()
                .All(Constraint.RelativeSphere(20), 
                    Constraint.Component<PlayerInfo.Component>())
                .Result<Position.Component, PlayerInfo.Component>();

            var miniMapConstraint = Interest.Query()
                .All(Constraint.RelativeBox(50, double.PositiveInfinity, 50),
                    Constraint.Component<MinimapRepresentation.Component>())
                .Result<Position.Component, MinimapRepresentation.Component>();

            var interest = new InterestComponent()
                .AddQuery<PlayerControls.Component>(playerConstraint)
                .AddQuery<PlayerControls.Component>(miniMapConstraint);
        }
        
        private static void TeamsExample()
        {
            var isblue = true;
            //some logic to determine which team

            var teamQuery = Interest.Query()
                .ComponentConstraint(isblue ? BlueTeam.ComponentId : RedTeam.ComponentId);
            
            var interest = new InterestComponent()
                .AddQuery<PlayerControls.Component>(teamQuery);
        }

        private static void FrequencyExample()
        {
            var playerConstraint = Interest.Query()
                .All(Constraint.RelativeSphere(20), 
                    Constraint.Component<PlayerInfo.Component>())
                .Frequency(20)
                .Result<Position.Component, PlayerInfo.Component>();

            var miniMapConstraint = Interest.Query()
                .All(Constraint.RelativeBox(50, double.PositiveInfinity, 50),
                    Constraint.Component<MinimapRepresentation.Component>())
                .Frequency(1)
                .Result<Position.Component, MinimapRepresentation.Component>();

            var interest = new InterestComponent()
                .AddQuery<PlayerControls.Component>(playerConstraint)
                .AddQuery<PlayerControls.Component>(miniMapConstraint);
        }
    }
}
