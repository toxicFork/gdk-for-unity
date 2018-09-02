using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Playground
{
    public class TileProvider
    {
        private readonly LruCache<TileKey, TileData> meshCache;
        private readonly Dictionary<TileKey, int> visibleMeshes;
        private readonly ITileVizualizer vizualizer;
        private int lastCleaned;

        public TileProvider(ITileVizualizer vizualizer, int capacity)
        {
            this.vizualizer = vizualizer;
            meshCache = new LruCache<TileKey, TileData>(capacity);
            visibleMeshes = new Dictionary<TileKey, int>();
        }

        public void LoadTile(TileKey key, Vector3 center)
        {
            var name = $"mesh_lod{key.LodLevel}_x{key.X}_z{key.Z}";

            if (visibleMeshes.ContainsKey(key))
            {
                visibleMeshes[key] = lastCleaned;
                return;
            }

            if (!meshCache.TryGetValue(key, out var tileData))
            {
                tileData = new TileData
                {
                    Mesh = Resources.Load<Mesh>(name),
                    Material = Resources.Load<Material>(name)
                };
                meshCache.Set(key, tileData);
            }

            visibleMeshes[key] = lastCleaned;
            vizualizer.VisualizeTile(key, tileData, center);
        }

        public void UnloadUnused()
        {
            visibleMeshes
                .Where(entry => entry.Value != lastCleaned)
                .ToList()
                .ForEach(entry =>
                {
                    visibleMeshes.Remove(entry.Key);
                    vizualizer.RemoveTile(entry.Key);
                    // Object.Destroy(entry.Value.Item1);
                });

            lastCleaned++;
        }
    }
}
