using System;
using System.Collections.Generic;
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

        public void Update(List<Vector3> points)
        {
            var lodLevel = terrainSettings.LodSettings.Count - 1;
            var step = terrainSettings.LodSettings[lodLevel].TileSize;

            for (var x = 0f; x < terrainSettings.TerrainSize; x += step)
            {
                for (var z = 0f; z < terrainSettings.TerrainSize; z += step)
                {
                    LoadTile(lodLevel, x, z, points);
                }
            }

            tileProvider.UnloadUnused();
        }

        private void LoadTile(int lodLevel, float x, float z, List<Vector3> points)
        {
            var lodSetting = terrainSettings.LodSettings[lodLevel];

            var center = lodSetting.GetCenter(new Vector3(x, 0, z)) + terrainSettings.Origin;
            var distance = ClosestPointDistance(points, center);

            if (distance > terrainSettings.MaxDistance)
            {
                return;
            }

            if (distance < lodSetting.MinDistance)
            {
                LoadTile(lodLevel - 1, x, z, points);
                LoadTile(lodLevel - 1, x + lodSetting.TileSize / 2, z, points);
                LoadTile(lodLevel - 1, x, z + lodSetting.TileSize / 2, points);
                LoadTile(lodLevel - 1, x + lodSetting.TileSize / 2, z + lodSetting.TileSize / 2, points);
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

        private static float ClosestPointDistance(List<Vector3> points, Vector3 center)
        {
            var distance = float.MaxValue;

            foreach (var point in points)
            {
                var newDistance = Mathf.Sqrt(Mathf.Pow(point.x - center.x, 2) + Mathf.Pow(point.z - center.z, 2));
                distance = Mathf.Min(newDistance, distance);
            }

            return distance;
        }

        public void Dispose()
        {
            tileProvider.Dispose();
        }
    }
}
