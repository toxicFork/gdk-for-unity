using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Playground
{
    public class TileProvider : IDisposable
    {
        private readonly LruCache<TileKey, TileData> tileCache;
        private readonly Dictionary<TileKey, int> visibleTiles;
        private readonly ITileVizualizer vizualizer;
        private readonly INameResolver nameResolver;
        private int lastCleaned;

        public TileProvider(ITileVizualizer vizualizer, INameResolver nameResolver, int capacity)
        {
            this.vizualizer = vizualizer;
            this.nameResolver = nameResolver;
            tileCache = new LruCache<TileKey, TileData>(capacity);
            visibleTiles = new Dictionary<TileKey, int>();
        }

        public void LoadTile(TileKey key, Vector3 center)
        {
            if (visibleTiles.ContainsKey(key))
            {
                visibleTiles[key] = lastCleaned;
                vizualizer.UpdateTile(key);
                return;
            }

            if (!tileCache.TryGetValue(key, out var tileData))
            {
                var name = nameResolver.GetNameForTile(key);

                tileData = new TileData
                {
                    Mesh = Resources.Load<Mesh>(name),
                    Material = Resources.Load<Material>(name)
                };

                tileCache.Set(key, tileData);
            }

            visibleTiles[key] = lastCleaned;
            vizualizer.LoadTile(key, tileData, center);
        }

        public void UnloadUnused()
        {
            visibleTiles
                .Where(entry => entry.Value != lastCleaned)
                .ToList()
                .ForEach(entry =>
                {
                    visibleTiles.Remove(entry.Key);
                    vizualizer.RemoveTile(entry.Key);
                });

            lastCleaned++;
        }

        public void Dispose()
        {
            vizualizer.Dispose();
        }
    }
}
