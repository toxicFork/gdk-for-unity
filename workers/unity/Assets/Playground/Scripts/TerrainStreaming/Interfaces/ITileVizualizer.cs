using System;
using UnityEngine;

namespace Playground
{
    public interface ITileVizualizer : IDisposable
    {
        void LoadTile(TileKey key, TileData mesh, Vector3 center);
        void UpdateTile(TileKey key);
        void RemoveTile(TileKey key);
    }
}
