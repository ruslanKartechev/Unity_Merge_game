using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Map
{
    public class MapLevelElement : MonoBehaviour
    {
        [SerializeField] private Image _backRend;
        [SerializeField] private TextMeshProUGUI _text;

        public void SetNumber(int level)
        {
            _text.text = $"{level}";
        }

        public void SetPassed(MapLevelSprites sprites)
        {
            _backRend.sprite = sprites.Passed;
            _backRend.color = sprites.ColorPassed;
        }

        public void SetCurrent(MapLevelSprites sprites)
        {
            _backRend.sprite = sprites.Current;
            _backRend.color = sprites.ColorCurrent;
        }

        public void SetFuture(MapLevelSprites sprites)
        {
            _backRend.sprite = sprites.Future;
            _backRend.color = sprites.ColorFuture;
        }
    }
}