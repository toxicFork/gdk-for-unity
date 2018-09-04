using System;

namespace Improbable.Gdk.TerrainStreaming
{
    public interface ITileVizualizer : IDisposable
    {
        void LoadTile(TileKey key, TileData mesh);
        void Update();
        void RemoveTile(TileKey key);
    }
}
