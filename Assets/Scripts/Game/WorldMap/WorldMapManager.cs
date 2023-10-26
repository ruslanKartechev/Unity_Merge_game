using System.Collections.Generic;
using UnityEngine;
using Common.Utils;

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
            {
                part.gameObject.SetActive(true);
            }
        }

        public void HideAll()
        {
            foreach (var part in _worldMapParts)
            {
                part.gameObject.SetActive(false);
            }
        }
        #endif        
    }
}