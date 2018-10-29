using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public struct LocalInputComponent : IComponentData
    {
        public Vector2 LeftStick;
        public Vector2 RightStick;
        public float CameraDistance;
        public BlittableBool Running;
        public BlittableBool ShootSmall;
        public BlittableBool ShootLarge;
    }
}
