using System.Collections.Generic;
using Common;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapBuilder : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _parts;
        [Space(10)]
        [SerializeField] private Vector3 _defaultCameraOffset;
        [Space(10)]
        [SerializeField] private bool _assignMaterials;
        [SerializeField] private Material _playerMaterial;
        [SerializeField] private List<Material> _enemyMaterial;
        
        private const string VegitationPointName = "VegitationPoint";
        private const string CameraPointName = "CameraPoint";
        private const string LevelPointName = "LevelPoint";
        
        
#if UNITY_EDITOR
        [ContextMenu("Build")]
        public void Build()
        {
            foreach (var go in _parts)
            {
                var gotr = go.transform;
                var script = go.GetComponent<WorldMapState>();
                if(script == null)
                    script = go.AddComponent<WorldMapState>();
                var vegPoint = go.transform.Find(VegitationPointName);
                if (vegPoint == null)
                {
                    vegPoint = new GameObject(VegitationPointName).transform;
                    vegPoint.position = gotr.position;
                    vegPoint.transform.parent = gotr;
                }
                script.VegitationSpawnPoint = vegPoint;

                var cameraPoint = gotr.Find(CameraPointName);
                if (cameraPoint == null)
                {
                    cameraPoint = new GameObject(CameraPointName).transform;
                    cameraPoint.position = gotr.position;
                    cameraPoint.transform.parent = gotr;
                }
                script.CameraPoint = new WorldMapCameraPoint(cameraPoint, _defaultCameraOffset);

                var levelPoint = gotr.Find(LevelPointName);
                if (levelPoint == null)
                {
                    levelPoint = new GameObject(LevelPointName).transform;
                    levelPoint.position = gotr.position;
                    levelPoint.transform.parent = gotr;
                }
                script.LevelSpawnPoint = levelPoint;
                
                if (_assignMaterials)
                {
                    script.PlayerMaterial = _playerMaterial;
                    script.EnemyMaterial = _enemyMaterial.Random();
                }
                UnityEditor.EditorUtility.SetDirty(go);
            }
        }

        [ContextMenu("Set camera offset")]
        public void SetCameraOffset()
        {
            foreach (var go in _parts)
            {
                var script = go.GetComponent<WorldMapState>();
                if(script == null)
                    script = go.AddComponent<WorldMapState>();
                if (script.CameraPoint != null)
                    script.CameraPoint.Offset = _defaultCameraOffset;
            }
        }
        
        
#endif

    }
}