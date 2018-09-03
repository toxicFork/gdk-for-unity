using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
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

            var settings = new TerrainSettings
            {
                TerrainSize = 1000,
                MaxDistance = 2000,
                Origin = World.GetExistingManager<WorkerSystem>().Origin,
                TileVizualizer = new DefaultTileVizualizer(),
                NameResolver = new DefaultNameResolver(),
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

            manager = new TerrainManager(settings);
        }

        protected override void OnUpdate()
        {
            var tree = new KdTree<Transform>(true);

            for (int i = 0; i < transformData.Length; i++)
            {
                tree.Add(transformData.Transform[i]);
            }

            manager.Update(tree);
        }
    }
}
