using UnityEngine;

namespace Game.UI.Map
{
    [System.Serializable]
    public class MapLevelSprites
    {
        [SerializeField] private Sprite _current;
        [SerializeField] private Sprite _passed;
        [SerializeField] private Sprite _future;

        public Sprite Current => _current;
        public Sprite Passed => _passed;
        public Sprite Future => _future;
        
    }
}