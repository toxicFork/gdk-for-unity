using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public class LodLoader : ComponentSystem
    {
        private struct TransformData
        {
            public readonly int Length;
            [ReadOnly] public ComponentArray<Transform> Transform;
        }

        [Inject] private TransformData transformData;

        private readonly List<LodSetting> lodSettings = new List<LodSetting>();
        private readonly TileProvider tileProvider = new TileProvider(new SimpleTileVizualizer(20), 20);
        private TerrainSettings terrainSettings;

        protected override void OnCreateManager(int capacity)
        {
            lodSettings.Add(new LodSetting
            {
                TileSize = 125,
                MinDistance = 0
            });

            lodSettings.Add(new LodSetting
            {
                TileSize = 250,
                MinDistance = 250
            });

            lodSettings.Add(new LodSetting
            {
                TileSize = 500,
                MinDistance = 500
            });

            lodSettings.Add(new LodSetting
            {
                TileSize = 1000,
                MinDistance = 1000
            });

            terrainSettings = new TerrainSettings
            {
                TerrainSize = 1000,
                MaxDistance = 1000
            };
        }

        protected override void OnUpdate()
        {
            var transforms = new KdTree<Transform>(true);

            for (int i = 0; i < transformData.Length; i++)
            {
                transforms.Add(transformData.Transform[i]);
            }

            var lodLevel = lodSettings.Count - 1;
            var step = GetSettingForLevel(lodLevel).TileSize;

            for (var x = 0f; x < terrainSettings.TerrainSize; x += step)
            {
                for (var z = 0f; z < terrainSettings.TerrainSize; z += step)
                {
                    LoadTile(lodLevel, x, z, transforms);
                }
            }

            tileProvider.UnloadUnused();
        }

        private LodSetting GetSettingForLevel(int lodLevel)
        {
            return lodSettings[lodLevel];
        }

        private void LoadTile(int lodLevel, float x, float z, KdTree<Transform> transforms)
        {
            var lodSetting = GetSettingForLevel(lodLevel);

            var center = lodSetting.GetCenter(new Vector3(x, 0, z));
            var closest = transforms.FindClosest(center);
            var distance = Vector3.Distance(center, closest.position);

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
    }
}
