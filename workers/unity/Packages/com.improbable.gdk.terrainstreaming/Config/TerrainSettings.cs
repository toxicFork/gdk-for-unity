using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.TerrainStreaming
{
    public struct TerrainSettings
    {
        public float TerrainSize;
        public float MaxDistance;
        public Vector3 Origin;

        public int TileProviderCacheSize;
        public float CacheTerrainAheadDistance;

        public ITileVizualizer TileVizualizer;
        public INameResolver NameResolver;

        public List<LodSetting> LodSettings;
    }
}
