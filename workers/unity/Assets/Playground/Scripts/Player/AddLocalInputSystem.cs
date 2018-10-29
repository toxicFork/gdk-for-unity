using Improbable.Gdk.Core;
using Unity.Entities;

namespace Playground
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(LocalPlayerInputSync))]
    public class AddLocalInputSystem : ComponentSystem
    {
        private struct PlayerInputData
        {
            public readonly int Length;
            public readonly EntityArray Entity;
            public ComponentDataArray<PlayerInput.Component> PlayerInput;
        }

        [Inject] private PlayerInputData playerInputData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < playerInputData.Length; i++)
            {
                PostUpdateCommands.AddComponent(playerInputData.Entity[i], typeof(LocalInputComponent));
            }

            Enabled = false;
        }
    }
}
