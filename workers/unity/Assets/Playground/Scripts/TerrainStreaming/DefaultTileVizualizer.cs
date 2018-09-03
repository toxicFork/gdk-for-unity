using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Playground
{
    public class DefaultTileVizualizer : ITileVizualizer
    {
        private readonly List<GameObject> freeGameObjects = new List<GameObject>();
        private readonly Dictionary<TileKey, GameObject> usedGameObjects = new Dictionary<TileKey, GameObject>();
        private const int Capacity = 20;

        public void LoadTile(TileKey key, TileData tileData, Vector3 center)
        {
            if (usedGameObjects.ContainsKey(key))
            {
                throw new InvalidOperationException("Cannot vizualize a tile which is already visible.");
            }

            GameObject tile;
            if (freeGameObjects.Count > 0)
            {
                tile = freeGameObjects[0];
                freeGameObjects.RemoveAt(0);
            }
            else
            {
                tile = new GameObject();
                tile.AddComponent<MeshCollider>();
                tile.AddComponent<MeshFilter>();
                tile.AddComponent<MeshRenderer>();
            }

            tile.name = $"Lod{key.LodLevel}_x{key.X}_z{key.Z}";
            tile.transform.position = center;

            var collider = tile.GetComponent<MeshCollider>();
            collider.sharedMesh = tileData.Mesh;

            var filter = tile.GetComponent<MeshFilter>();
            filter.sharedMesh = tileData.Mesh;

            var renderer = tile.GetComponent<MeshRenderer>();
            renderer.material = tileData.Material;

            usedGameObjects[key] = tile;
        }

        public void UpdateTile(TileKey key)
        {
        }

        public void RemoveTile(TileKey key)
        {
            if (!usedGameObjects.TryGetValue(key, out var tile))
            {
                throw new InvalidOperationException("Cannot remove a tile which doesn't exist.");
            }

            if (freeGameObjects.Count == Capacity)
            {
                DestroyGameObject(tile);
                usedGameObjects.Remove(key);
                return;
            }

            tile.name = "Unused terrain";

            var collider = tile.GetComponent<MeshCollider>();
            collider.sharedMesh = null;

            var filter = tile.GetComponent<MeshFilter>();
            filter.mesh = null;

            usedGameObjects.Remove(key);
            freeGameObjects.Add(tile); 
        }

        private void DestroyGameObject(GameObject gameObject)
        {
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                Object.DestroyImmediate(gameObject);
            }
            else
#endif
            {
                Object.Destroy(gameObject);
            }
        }

        public void Dispose()
        {
            foreach (var tile in usedGameObjects)
            {
                DestroyGameObject(tile.Value);
            }

            foreach (var tile in freeGameObjects)
            {
                DestroyGameObject(tile);
            }

            usedGameObjects.Clear();
            freeGameObjects.Clear();
        }
    }
}
