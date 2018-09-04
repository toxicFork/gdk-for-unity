namespace Improbable.Gdk.TerrainStreaming
{
    public interface INameResolver
    {
        string GetNameForTile(TileKey key);
    }
}
