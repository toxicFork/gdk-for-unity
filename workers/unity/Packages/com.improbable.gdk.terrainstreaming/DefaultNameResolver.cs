namespace Improbable.Gdk.TerrainStreaming
{
    public class DefaultNameResolver : INameResolver
    {
        public string GetNameForTile(TileKey key)
        {
            return $"mesh_lod{key.LodLevel}_x{key.X}_z{key.Z}";
        }
    }
}
