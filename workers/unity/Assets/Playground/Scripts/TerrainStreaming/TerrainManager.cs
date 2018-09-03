using System;
using UnityEngine;

namespace Playground
{
    public class TerrainManager : IDisposable
    {
        private readonly TileProvider tileProvider;
        private readonly TerrainSettings terrainSettings;

        public TerrainManager(TerrainSettings settings)
        {
            terrainSettings = settings;
            tileProvider = new TileProvider(terrainSettings.TileVizualizer, terrainSettings.NameResolver,
                terrainSettings.TileProviderCacheSize);
        }

        public void Update(KdTree<Transform> transforms)
        {
            var lodLevel = terrainSettings.LodSettings.Count - 1;
            var step = terrainSettings.LodSettings[lodLevel].TileSize;

            for (var x = 0f; x < terrainSettings.TerrainSize; x += step)
            {
                for (var z = 0f; z < terrainSettings.TerrainSize; z += step)
                {
                    LoadTile(lodLevel, x, z, transforms);
                }
            }

            tileProvider.UnloadUnused();
        }

        private void LoadTile(int lodLevel, float x, float z, KdTree<Transform> transforms)
        {
            var lodSetting = terrainSettings.LodSettings[lodLevel];

            var center = lodSetting.GetCenter(new Vector3(x, 0, z)) + terrainSettings.Origin;
            var closest = transforms.FindClosest(center);
            var distance = Distance(center, closest.position);

            if (distance > terrainSettings.MaxDistance)
            {
                return;
            }

            if (distance < lodSetting.MinDistance)
            {
                LoadTile(lodLevel - 1, x, z, transforms);
                LoadTile(lodLevel - 1, x + lodSetting.TileSize / 2, z, transforms);
                LoadTile(lodLevel - 1, x, z + lodSetting.TileSize / 2, transforms);
                LoadTile(lodLevel - 1, x + lodSetting.TileSize / 2, z + lodSetting.TileSize / 2, transforms);
            }
            else
            {
                var key = new TileKey
                {
                    LodLevel = lodLevel,
                    X = (int) (x / lodSetting.TileSize),
                    Z = (int) (z / lodSetting.TileSize)
                };

                tileProvider.LoadTile(key, center);
            }
        }

        private static float Distance(Vector3 a, Vector3 b)
        {
            return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z));
        }

        public void Dispose()
        {
            tileProvider.Dispose();
        }
    }
}
