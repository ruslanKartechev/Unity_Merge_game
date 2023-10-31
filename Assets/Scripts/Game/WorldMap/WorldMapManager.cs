using System.Collections.Generic;
using UnityEngine;
using Common.Utils;
using UnityEditor;

namespace Game.WorldMap
{
    public class WorldMapManager : MonoBehaviour
    {
        [SerializeField] private MapCamera _camera;
        [SerializeField] private float _cameraMoveDuration = 1f;
        [SerializeField] private List<WorldMapPart> _worldMapParts;

        private int CorrectIndex(int level)
        {
            if(level < 0)
                level = 0;
            var subed = false;
            while (level >= _worldMapParts.Count)
            {
                subed = true;
                level -= _worldMapParts.Count;
            }
            if(subed && level == 0)
                level++;
               
            return level;
        }
        
        public void ShowLevel(int level)
        {
            var totalLevel = level;
            var currentIndex = CorrectIndex(level);
            var current = _worldMapParts[currentIndex];
            current.Show();
            current.SetEnemyTerritory();
            current.ShowLevelNumber(totalLevel);
            current.SpawnLevelEnemies(totalLevel);
            
            var camPoint = current.CameraPoint;
            // _camera.SetFarPoint(camPoint);
            // _camera.MoveFarToClose(camPoint,_cameraMoveDuration);
            _camera.SetClosePoint(camPoint);
            
            var prevLevel = level - 1;
            if(prevLevel < 0)
                return;
            var prevPartIndex = CorrectIndex(prevLevel);
            var prevPart = _worldMapParts[prevPartIndex];
            prevPart.Show();
            prevPart.ShowLevelNumber(prevLevel);
            prevPart.SetPlayerTerritory();
            // spawn player pack here

            for (var i = 0; i < prevPartIndex; i++)
            {
                _worldMapParts[i].ShowLevelNumber(i);
                _worldMapParts[i].SetPlayerTerritory();
            }
       
            for (var i = currentIndex + 1; i < _worldMapParts.Count; i++)
            {
                _worldMapParts[i].SetEnemyTerritory();
                _worldMapParts[i].HideLevelNumber();
            }
        }
        
        
        #if UNITY_EDITOR
        public void GetParts()
        {
            _worldMapParts.Clear();
            _worldMapParts = HierarchyUtils.GetFromAllChildren<WorldMapPart>(transform);
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void ShowAll()
        {
            foreach (var part in _worldMapParts)
                part.gameObject.SetActive(true);
        }

        public void HideAll()
        {
            foreach (var part in _worldMapParts)
                part.gameObject.SetActive(false);
        }

        public void SetAllAsEnemyTerritory()
        {
            foreach (var part in _worldMapParts)
                part.SetEnemyTerritory();
        }

        public void SetAllAsPlayerTerritory()
        {
            foreach (var part in _worldMapParts)
                part.SetPlayerTerritory();
        }
        
        
        [ContextMenu("Rename states")]
        public void RenameStates()
        {
            var num = 1;
            foreach (var part in _worldMapParts)
            {
                if(part == null)
                    continue;
                part.gameObject.name = $"State {num}";
                num++;
            }
        }
        
        
        [Space(20)]
        [SerializeField] private bool _doDraw;
        [SerializeField] private int _fontSize = 20;

        public void OnDrawGizmos()
        {
            if (!_doDraw)
                return;
            var num = 1;
            var style = new GUIStyle(GUI.skin.GetStyle("Label"));
            style.fontSize = _fontSize;
            style.fontStyle = FontStyle.Bold;
            var oldColor = Handles.color;
            Handles.color = Color.red;
            foreach (var part in _worldMapParts)
            {
                Handles.Label(part.transform.position, $"{num}", style);
                num++;
            }

            Handles.color = oldColor;
        }
        #endif        
    }
}