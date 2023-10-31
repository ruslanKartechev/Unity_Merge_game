using UnityEngine;

namespace Game.UI.Map
{
    [System.Serializable]
    public class MapLevelSprites
    {
        [SerializeField] private Sprite _current;
        [SerializeField] private Sprite _passed;
        [SerializeField] private Sprite _future;
        
        [SerializeField] private Color _colorCurrent;
        [SerializeField] private Color _colorPassed;
        [SerializeField] private Color _colorFuture;

        public Color ColorCurrent => _colorCurrent;
        public Color ColorPassed => _colorPassed;
        public Color ColorFuture => _colorFuture;
        

        public Sprite Current => _current;
        public Sprite Passed => _passed;
        public Sprite Future => _future;
        
    }
}