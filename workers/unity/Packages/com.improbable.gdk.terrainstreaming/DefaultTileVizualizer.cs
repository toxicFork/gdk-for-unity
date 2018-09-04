using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.TerrainStreaming
{
    public class DefaultTileVizualizer : ITileVizualizer
    {
        private readonly List<GameObject> freeGameObjects = new List<GameObject>();
        private readonly Dictionary<TileKey, GameObject> usedGameObjects = new Dictionary<TileKey, GameObject>();
        private readonly Dictionary<TileKey, TileData> visibleTiles = new Dictionary<TileKey, TileData>();
        private const int Capacity = 20;

        public void LoadTile(TileKey key, TileData tileData)
        {
            if (visibleTiles.ContainsKey(key))
            {
                throw new InvalidOperationException("Cannot vizualize a tile which is already visible.");
            }

            if (key.LodLevel == 0)
            {
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
                }

                tile.name = $"Lod{key.LodLevel}_x{key.X}_z{key.Z}";
                tile.transform.position = key.Center;

                var collider = tile.GetComponent<MeshCollider>();
                collider.sharedMesh = tileData.Mesh;

                usedGameObjects[key] = tile;
            }

            visibleTiles[key] = tileData;
        }

        public void Update()
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);


            foreach (var tile in visibleTiles)
            {
                var bounds = new Bounds(tile.Key.Center, tile.Value.Mesh.bounds.size);

                if (Application.isEditor || GeometryUtility.TestPlanesAABB(planes, bounds))
                {
                    Graphics.DrawMesh(tile.Value.Mesh, tile.Key.Center, Quaternion.identity, tile.Value.Material, 0);
                }
            }
        }

        public void RemoveTile(TileKey key)
        {
            if (!visibleTiles.ContainsKey(key))
            {
                throw new InvalidOperationException("Cannot remove a tile which doesn't exist.");
            }

            visibleTiles.Remove(key);

            if (usedGameObjects.TryGetValue(key, out var tile))
            {
                if (freeGameObjects.Count == Capacity)
                {
                    DestroyGameObject(tile);
                    usedGameObjects.Remove(key);
                    return;
                }

                tile.name = "Unused terrain";

                var collider = tile.GetComponent<MeshCollider>();
                collider.sharedMesh = null;

                usedGameObjects.Remove(key);
                freeGameObjects.Add(tile);
            }
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

