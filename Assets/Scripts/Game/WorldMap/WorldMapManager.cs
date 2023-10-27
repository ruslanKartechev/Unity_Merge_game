using System.Collections.Generic;
using UnityEngine;
using Common.Utils;
using UnityEditor;

namespace Game.WorldMap
{
    public class WorldMapManager : MonoBehaviour
    {
        [SerializeField] private MapCamera _camera;
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
            var partIndex = CorrectIndex(level);
            var current = _worldMapParts[partIndex];
            current.Show();
            current.SetEnemyTerritory();
            current.ShowLevelNumber(totalLevel);
            current.SpawnLevelEnemies(totalLevel);
            _camera.SetPosition(current.CameraPoint);
            
            var prevLevel = level - 1;
            if(prevLevel < 0)
                return;
            var prevPartIndex = CorrectIndex(prevLevel);
            
            var prevPart = _worldMapParts[prevPartIndex];
            prevPart.Show();
            prevPart.ShowLevelNumber(prevLevel);
            prevPart.SetPlayerTerritory();
            // spawn player pack here
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

        public void SetAllPlayerAndVegitation()
        {
            
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