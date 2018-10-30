using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(LocalPlayerInputSync))]
    public class MobilePlayerInputSystem : ComponentSystem
    {
        private readonly VirtualJoystick movementJoystick;
        private readonly VirtualJoystick cameraJoystick;

        private readonly RectTransform movementJoystickBoundaries;
        private readonly RectTransform cameraJoystickBoundaries;

        private HashSet<int> oldTouchSet;
        private HashSet<int> newTouchSet;

        public MobilePlayerInputSystem(VirtualJoystick movementJoystick, VirtualJoystick cameraJoystick)
        {
            this.movementJoystick = movementJoystick;
            this.cameraJoystick = cameraJoystick;

            movementJoystickBoundaries = movementJoystick.GetComponent<Image>().rectTransform;
            cameraJoystickBoundaries = cameraJoystick.GetComponent<Image>().rectTransform;

            oldTouchSet = new HashSet<int>();
            newTouchSet = new HashSet<int>();
        }

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
                var touches = GetNonJoystickTouches();
                input.LeftStick = movementJoystick.InputDirection;
                input.RightStick = cameraJoystick.InputDirection;
                // Camera zoom and running are not implemented for mobile
                input.ShootSmall = touches == 1;
                input.ShootLarge = touches >= 2;
                inputData.PlayerInput[i] = input;
                Debug.Log($"Movement Joystick {movementJoystick}");
                Debug.Log($"Camera Joystick {cameraJoystick}");
                Debug.Log($"Movement Joystick {movementJoystick.InputDirection}");
                Debug.Log($"Camera Joystick {cameraJoystick.InputDirection}");
            }
        }


        private int GetNonJoystickTouches()
        {
            newTouchSet.Clear();
            foreach (var touch in Input.touches)
            {
                if (oldTouchSet.Contains(touch.fingerId))
                {
                    newTouchSet.Add(touch.fingerId);
                }
            }

            var temp = newTouchSet;
            newTouchSet = oldTouchSet;
            oldTouchSet = temp;

            var nonJoystickTouches = 0;
            foreach (var touch in Input.touches)
            {
                if (!oldTouchSet.Contains(touch.fingerId) && !(RectTransformUtility.RectangleContainsScreenPoint(
                        movementJoystickBoundaries, touch.position,
                        null) ||
                    RectTransformUtility.RectangleContainsScreenPoint(cameraJoystickBoundaries, touch.position,
                        null)))
                {
                    nonJoystickTouches++;
                }

                oldTouchSet.Add(touch.fingerId);
            }

            return nonJoystickTouches;
        }
    }
}
