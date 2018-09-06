using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.TerrainStreaming
{
    public struct TileProviderSettings
    {
        public int TileProviderCacheSize;
        public ITileVizualizer TileVizualizer;
        public INameResolver NameResolver;
    }

    public struct StreamingSettings
    {
        public float TerrainSize;
        public float MaxDistance;
        public Vector3 Origin;
        public float CacheTerrainAheadDistance;
        public List<LodSetting> LodSettings;
    }
}
