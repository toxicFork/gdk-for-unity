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
            var playerQuery = Interest
                .Query(Constraint.All(
                    Constraint.RelativeSphere(20),
                    Constraint.Component<PlayerInfo.Component>()))
                .Filter(Position.ComponentId, PlayerInfo.ComponentId);

            var miniMapQuery = Interest
                .Query(Constraint.All(
                    Constraint.RelativeBox(50, double.PositiveInfinity, 50),
                    Constraint.Component<MinimapRepresentation.Component>()))
                .Filter(Position.ComponentId, MinimapRepresentation.ComponentId);

            var interest = new InterestBuilder()
                .AddQueries<PlayerControls.Component>(playerQuery, miniMapQuery);
        }

        private static void TeamsExample()
        {
            var isblue = true;
            //some logic to determine which team

            var teamQuery = Interest
                .Query(Constraint.Component(isblue ? BlueTeam.ComponentId : RedTeam.ComponentId));

            var interest = new InterestBuilder()
                .AddQuery<PlayerControls.Component>(teamQuery);
        }

        private static void FrequencyExample()
        {
            var playerQuery = Interest
                .Query(Constraint.All(
                    Constraint.RelativeSphere(20),
                    Constraint.Component<PlayerInfo.Component>()))
                .MaxFrequencyHz(20)
                .Filter(Position.ComponentId, PlayerInfo.ComponentId);

            var miniMapQuery = Interest
                .Query(Constraint.All(
                    Constraint.RelativeBox(50, double.PositiveInfinity, 50),
                    Constraint.Component<MinimapRepresentation.Component>()))
                .MaxFrequencyHz(1)
                .Filter(Position.ComponentId, MinimapRepresentation.ComponentId);

            var interest = new InterestBuilder()
                .AddQueries<PlayerControls.Component>(playerQuery, miniMapQuery);
        }
    }
}
