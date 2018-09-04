using System;
using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.TerrainStreaming
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

            tileProvider.Update();
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
                var key = GenerateKey(lodLevel, x, z, lodSetting);
                tileProvider.LoadTile(key);
            }

            if (lodSetting.MinDistance <= distance &&
                distance < lodSetting.MinDistance + terrainSettings.CacheTerrainAheadDistance && lodLevel != 0)
            {
                for (int i = 0; i <= 1; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        var newX = x + i * lodSetting.TileSize / 2;
                        var newZ = z + j * lodSetting.TileSize / 2;
                        var subLodSetting = terrainSettings.LodSettings[lodLevel - 1];

                        var key = GenerateKey(lodLevel - 1, newX, newZ, subLodSetting);

                        tileProvider.LoadIntoCache(key);
                    }
                }
            }

            if (lodSetting.MinDistance - terrainSettings.CacheTerrainAheadDistance <= distance &&
                distance < lodSetting.MinDistance)
            {
                var key = GenerateKey(lodLevel, x, z, lodSetting);
                tileProvider.LoadIntoCache(key);
            }
        }

        private TileKey GenerateKey(int lodLevel, float x, float z, LodSetting lodSetting)
        {
            return new TileKey
            {
                LodLevel = lodLevel,
                X = (int) (x / lodSetting.TileSize),
                Z = (int) (z / lodSetting.TileSize),
                Center = lodSetting.GetCenter(new Vector3(x, 0, z)) + terrainSettings.Origin
            };
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
