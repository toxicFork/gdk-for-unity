using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TerrainStreaming
{
    public class TileStreamerSystem : ComponentSystem
    {
        private struct TransformData
        {
            public readonly int Length;
            [ReadOnly] public ComponentArray<Transform> Transform;
        }

        [Inject] private TransformData transformData;

        private TerrainManager manager;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            var streamingSettings = new StreamingSettings
            {
                TerrainSize = 1000,
                MaxDistance = 2000,
                CacheTerrainAheadDistance = 20,
                Origin = World.GetExistingManager<WorkerSystem>().Origin,
                LodSettings = new List<LodSetting>
                {
                    new LodSetting
                    {
                        TileSize = 125,
                        MinDistance = 0
                    },
                    new LodSetting
                    {
                        TileSize = 250,
                        MinDistance = 250
                    },
                    new LodSetting
                    {
                        TileSize = 500,
                        MinDistance = 500
                    },
                    new LodSetting
                    {
                        TileSize = 1000,
                        MinDistance = 1000
                    }
                }
            };

            var managerSettings = new TileProviderSettings
            {
                TileProviderCacheSize = 50,
                TileVizualizer = new DefaultTileVizualizer(),
                NameResolver = new DefaultNameResolver(),
            };

            manager = new TerrainManager(managerSettings, streamingSettings);
        }

        protected override void OnUpdate()
        {
            var tree = new List<Vector3>();

            for (int i = 0; i < transformData.Length; i++)
            {
                tree.Add(transformData.Transform[i].position);
            }

            manager.Update(tree);
        }
    }
}
