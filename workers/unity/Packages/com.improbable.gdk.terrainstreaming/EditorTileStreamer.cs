using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.TerrainStreaming
{
    [ExecuteInEditMode]
    public class EditorTileStreamer : MonoBehaviour
    {
        private TerrainManager manager;
        private Vector3 lastCameraPosition;

        void OnEnable()
        {
            var settings = new TerrainSettings
            {
                TerrainSize = 1000,
                MaxDistance = 2000,
                TileProviderCacheSize = 50,
                CacheTerrainAheadDistance = 20,
                Origin = new Vector3(0, 0, 0),
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

            EditorApplication.update += UIChange;
        }

        void OnDisable()
        {
            EditorApplication.update -= UIChange;
            manager.Dispose();
        }

        void UIChange()
        {
            var view = SceneView.lastActiveSceneView;
            if (view != null)
            {
                lastCameraPosition = view.camera.transform.position;
            }

            manager.Update(new List<Vector3> { lastCameraPosition });
        }
    }
}
