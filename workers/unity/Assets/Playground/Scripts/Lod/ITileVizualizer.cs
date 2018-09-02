using UnityEngine;

namespace Playground
{
    public interface ITileVizualizer
    {
        void VisualizeTile(TileKey key, TileData mesh, Vector3 center);
        void RemoveTile(TileKey key);
    }
}
