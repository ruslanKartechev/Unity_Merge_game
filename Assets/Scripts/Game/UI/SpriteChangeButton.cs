using Common.UIEffects;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SpriteChangeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        // [SerializeField] private Sprite _usual;
        // [SerializeField] private Sprite _active;
        [SerializeField] private Image _image;
        [SerializeField] private Image _imageOverlay;
        
        [SerializeField] private ScaleEffect _scaleEffect;

        public Button Btn => _button;
        
        public void SetUsual()
        {
            if (_imageOverlay == null)
                return;
            _imageOverlay.enabled = false;
        }

        public void SetActive()
        {
            if (_imageOverlay == null)
                return;
            _imageOverlay.enabled = true;
        }

        public void Scale()
        {
            _scaleEffect.Play();   
        }
    }
}