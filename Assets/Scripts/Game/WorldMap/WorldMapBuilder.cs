using System.Collections.Generic;
using Common;
using UnityEditor;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapBuilder : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _parts;
        [Space(10)] 
        [SerializeField] private WorldMapCameraPoint _worldMapCameraPointPrefab;
        [Space(10)]
        [SerializeField] private bool _assignMaterials;
        [SerializeField] private Material _playerMaterial;
        [SerializeField] private List<Material> _enemyMaterial;
        [Space(10)] 
        [SerializeField] private bool _spawnLevelNumber;
        [SerializeField] private WorldMapLevelNumber _levelNumberPrefab;
        [SerializeField] private Vector3 _levelNumLocalScale;

        
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
            if(_spawnLevelNumber)
                SpawnLevelNumbers();
        }

        [ContextMenu("Destroy All Level Numbers")]
        public void DestroyLevelNumbers()
        {
            foreach (var go in _parts)
            {
                var gotr = go.transform;
                var script = go.GetComponent<WorldMapState>();
                if(script == null)
                    continue;
                var levelNum = script.MapLevelNumber;
                if (levelNum == null)
                    levelNum = gotr.GetComponentInChildren<WorldMapLevelNumber>();
                if(levelNum != null)
                    DestroyImmediate(levelNum.gameObject);
                UnityEditor.EditorUtility.SetDirty(go);
            }
        }

        [ContextMenu("Spawn All Level Numbers")]
        public void SpawnLevelNumbers()
        {
            foreach (var go in _parts)
            {
                var gotr = go.transform;
                var script = go.GetComponent<WorldMapState>();
                if (script == null)
                    continue;
                var levelNum = gotr.GetComponentInChildren<WorldMapLevelNumber>();
                if (levelNum == null)
                {
                    levelNum = PrefabUtility.InstantiatePrefab(_levelNumberPrefab) as WorldMapLevelNumber;
                    levelNum.transform.SetParent(gotr);
                    levelNum.transform.position = gotr.position + Vector3.up;
                    script.MapLevelNumber = levelNum;
                }
                UnityEditor.EditorUtility.SetDirty(go);
            }
        }
        
        [ContextMenu("Set Level Number Scale")]
        public void SetLevelNumberScale()
        {
            foreach (var go in _parts)
            {
                var gotr = go.transform;
                var script = go.GetComponent<WorldMapState>();
                if (script == null)
                    continue;
                
                var levelNum = gotr.GetComponentInChildren<WorldMapLevelNumber>();
                if (levelNum == null)
                    continue;
                levelNum.transform.localScale = _levelNumLocalScale;
                UnityEditor.EditorUtility.SetDirty(go);
            }
        }
        
        [ContextMenu("Spawn Level Point")]
        public void SpawnLevelPoints()
        {
            foreach (var go in _parts)
            {
                var gotr = go.transform;
                var script = go.GetComponent<WorldMapState>();
                if (script == null)
                    continue;
                var levelPoint = gotr.Find(LevelPointName);
                if (levelPoint == null)
                {
                    levelPoint = new GameObject(LevelPointName).transform;
                    levelPoint.position = gotr.position;
                    levelPoint.transform.parent = gotr;
                }
                script.LevelSpawnPoint = levelPoint;
                UnityEditor.EditorUtility.SetDirty(go);
            }
        }
        
                
        [ContextMenu("Destroy Old Camera Points")]
        public void DestroyOldLevelPoints()
        {
            foreach (var go in _parts)
            {
                var gotr = go.transform;
                var script = go.GetComponent<WorldMapState>();
                if (script == null)
                    continue;
                var levelPoint = gotr.Find(CameraPointName);
                if(levelPoint != null)
                    DestroyImmediate(levelPoint.gameObject);
                UnityEditor.EditorUtility.SetDirty(go);
            }
        }
        
        [ContextMenu("Spawn NEW Camera Points")]
        public void SpawnCameraPoints()
        {
            foreach (var go in _parts)
            {
                var script = go.GetComponent<WorldMapState>();
                if(script == null)
                    script = go.AddComponent<WorldMapState>();
                if (script.CameraPoint != null)
                {
                    DestroyImmediate(script.CameraPoint.gameObject);
                }
                SpawnCameraPoint(script);
            }
        }
        
        
        [ContextMenu("Spawn Missing Camera Points")]
        public void SpawnMissingCameraPoints()
        {
            foreach (var go in _parts)
            {
                var script = go.GetComponent<WorldMapState>();
                if(script == null)
                    script = go.AddComponent<WorldMapState>();
                if (script.CameraPoint != null)
                    continue;
                SpawnCameraPoint(script);
            }
        }
        
        
        

        private void SpawnCameraPoint(WorldMapState script)
        {
            var instance = PrefabUtility.InstantiatePrefab(_worldMapCameraPointPrefab) as WorldMapCameraPoint;
            instance.transform.parent = script.transform;
            instance.transform.position = script.transform.position;
            script.CameraPoint = instance;
        }
#endif
    }
}