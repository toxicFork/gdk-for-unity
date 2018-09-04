using Unity.Entities;

namespace Improbable.Gdk.TerrainStreaming
{
    public static class TerrainStreamingConfig
    {
        public static void AddSystems(World world)
        {
            world.GetOrCreateManager<TileStreamerSystem>();
        }
    }
}
