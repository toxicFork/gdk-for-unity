using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    // todo can make this a union but it won't make the struct smaller
    internal readonly struct EntityOp
    {
        public readonly OpType OpType;
        public readonly long EntityId;
        public readonly uint ComponentId;
        public readonly uint CommandId;

        public EntityOp(OpType opType, long entityId = 0, uint componentId = 0, uint commandId = 0)
        {
            OpType = opType;
            EntityId = entityId;
            ComponentId = componentId;
            CommandId = commandId;
        }
    }
}
