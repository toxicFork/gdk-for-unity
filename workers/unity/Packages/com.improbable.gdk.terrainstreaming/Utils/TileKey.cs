using UnityEngine;

namespace Improbable.Gdk.TerrainStreaming
{
    public struct TileKey
    {
        public int LodLevel;
        public int X;
        public int Z;
        public Vector3 Center;
    }
}
