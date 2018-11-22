using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    public class SpatialOSComponent : MonoBehaviour
    {
        public EntityId SpatialEntityId;
        public WorkerSystem Worker;
        public World World;
        public Entity Entity;
    }
}
