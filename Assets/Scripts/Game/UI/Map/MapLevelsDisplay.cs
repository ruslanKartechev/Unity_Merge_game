using UnityEngine;
using System.Collections.Generic;
using Common.Utils;

namespace Game.UI.Map
{
    public class MapLevelsDisplay : MonoBehaviour
    {
        private const int SpawnCount = 7;
        [SerializeField] private float _spacing;
        [SerializeField] private float _size;
        [SerializeField] private RectTransform _center;
        [SerializeField] private MapLevelElement _elementPrefab;
        [SerializeField] private List<MapLevelElement> _spawned;
        [SerializeField] private MapLevelSprites _sprites;
        
#if UNITY_EDITOR
        public int DebugSpawnLevel = 1;
#endif

        // input 'level' as Index
        public void ShowLevel(int level)
        {
            #if UNITY_EDITOR
            ClearSpawned(); 
            #endif
            var elements = SpawnElements(SpawnCount);
            _spawned = elements;
            var currentNumber = level + 1;
            // number is level number as displayed to the player
            var number = 1;
            if (level > 2)
                number = currentNumber -2;
            for (var i = 0; i < SpawnCount; i++)
            {
                var element = elements[i];
                element.SetNumber(number);
                if(number == currentNumber)
                    element.SetCurrent(_sprites);
                else if (number < currentNumber)
                    element.SetPassed(_sprites);
                else if (number > currentNumber)
                    element.SetFuture(_sprites);
                number++;
            }
        }

        public List<MapLevelElement> SpawnElements(int count)
        {
            float leftIndent = count / 2;
            if (count % 2 == 0)
                leftIndent -= .5f;
            var space = _size + _spacing / 2f;
            var centerX = _center.position.x - leftIndent * (space);
            var centerY = _center.position.y;
            var elements = new List<MapLevelElement>();
            for (var i = 0; i < count; i++)
            {
                var pos = new Vector3(centerX + i * space, centerY, 0);
                var element = SpawnElement();
                element.gameObject.name = $"Element {i + 1}";
                element.transform.position = pos;
                elements.Add(element);
            }
            return elements;
        }
        
        public MapLevelElement SpawnElement()
        {
            #if UNITY_EDITOR
            if (Application.isPlaying)
                return Instantiate(_elementPrefab, transform);
            return UnityEditor.PrefabUtility.InstantiatePrefab(_elementPrefab, transform) as MapLevelElement;
#else
            return Instantiate(_elementPrefab, transform);
#endif
        }

        public void ClearSpawned()
        {
            for (var i = 0; i < _spawned.Count; i++)
            {
                if(_spawned[i] != null)
                    ObjectDetroyer.Clear(_spawned[i].gameObject);
            }
            _spawned.Clear();   
        }
    }
}