using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Playground
{
    public class TileProvider : IDisposable
    {
        private readonly Queue<TileKey> tilesToCache;
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
            tilesToCache = new Queue<TileKey>();
        }

        public void LoadTile(TileKey key)
        {
            if (visibleTiles.ContainsKey(key))
            {
                visibleTiles[key] = lastCleaned;
                return;
            }

            if (!tileCache.TryGetValue(key, out var tileData))
            {
                tileData = LoadIntoCacheImediately(key);
            }

            visibleTiles[key] = lastCleaned;
            vizualizer.LoadTile(key, tileData);
        }

        public void LoadIntoCache(TileKey key)
        {
            if (!tileCache.ContainsKey(key) && !tilesToCache.Contains(key))
            {
                tilesToCache.Enqueue(key);
            }
        }

        private TileData LoadIntoCacheImediately(TileKey key)
        {
            var name = nameResolver.GetNameForTile(key);

            var tileData = new TileData
            {
                Mesh = Resources.Load<Mesh>(name),
                Material = Resources.Load<Material>(name)
            };

            tileCache.Set(key, tileData);
            return tileData;
        } 

        public void Update()
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

            var tilesToLoad = 1;
            while (tilesToLoad > 0 && tilesToCache.Count > 0)
            {
                var key = tilesToCache.Dequeue();

                if (!tileCache.ContainsKey(key))
                {
                    LoadIntoCacheImediately(key);
                    tilesToLoad--;
                }
            }

            vizualizer.Update();
        }

        public void Dispose()
        {
            vizualizer.Dispose();
        }
    }
}
