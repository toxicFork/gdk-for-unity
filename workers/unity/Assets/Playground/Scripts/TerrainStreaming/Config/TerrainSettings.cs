using System.Collections.Generic;
using UnityEngine;

namespace Playground
{
    public struct TerrainSettings
    {
        public float TerrainSize;
        public float MaxDistance;
        public Vector3 Origin;

        public int TileProviderCacheSize;
        public ITileVizualizer TileVizualizer;
        public INameResolver NameResolver;

        public List<LodSetting> LodSettings;
    }
}
