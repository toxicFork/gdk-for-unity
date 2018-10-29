using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(LocalPlayerInputSync))]
    public class DesktopPlayerInputSystem : ComponentSystem
    {
        private struct InputData
        {
            public readonly int Length;
            public ComponentDataArray<LocalInputComponent> PlayerInput;
        }

        [Inject] private InputData inputData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < inputData.Length; i++)
            {
                var input = inputData.PlayerInput[i];
                input.LeftStick.x = Input.GetAxis("Horizontal");
                input.LeftStick.y = Input.GetAxis("Vertical");
                input.RightStick.x = Input.GetAxis("Mouse X");
                input.RightStick.y = Input.GetAxis("Mouse Y");
                input.CameraDistance = Input.GetAxis("Mouse ScrollWheel");
                input.Running = Input.GetKey(KeyCode.LeftShift);
                input.ShootSmall = Input.GetMouseButtonDown(0);
                input.ShootLarge = Input.GetMouseButtonDown(1);
                inputData.PlayerInput[i] = input;
            }
        }
    }
}
