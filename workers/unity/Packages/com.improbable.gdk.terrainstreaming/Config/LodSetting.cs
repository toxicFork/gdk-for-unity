using UnityEngine;

namespace Improbable.Gdk.TerrainStreaming
{
    public struct LodSetting
    {
        public float TileSize;
        public float MinDistance;

        public Vector3 GetCenter(Vector3 corner)
        {
            return new Vector3(corner.x + TileSize / 2, corner.y, corner.z + TileSize / 2);
        }
    }
}
