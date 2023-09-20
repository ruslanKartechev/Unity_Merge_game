using Common.UIEffects;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.StartScreen
{
    public class SpriteChangeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Sprite _usual;
        [SerializeField] private Sprite _active;
        [SerializeField] private Image _image;
        [SerializeField] private ScaleEffect _scaleEffect;

        public Button Btn => _button;
        public void SetUsual() => _image.sprite = _usual;

        public void SetActive() => _image.sprite = _active;

        public void Scale()
        {
            _scaleEffect.Play();   
        }
    }
}