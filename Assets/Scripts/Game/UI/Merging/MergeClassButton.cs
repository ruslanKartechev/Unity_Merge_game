using Common.UIEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeClassButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _colorPassive;
        [SerializeField] private Color _colorActive;
        [SerializeField] private Image _highlightImage;
        [SerializeField] private Button _button;
        [SerializeField] private ScaleEffect _scaleEffect;
        [Space(10)]
        [SerializeField] private Image _iconImage;
        [SerializeField] private Sprite _iconPassive;
        [SerializeField] private Sprite _iconActive;
        [Space(10)] 
        [SerializeField] private TextMeshProUGUI _number;
        [SerializeField] private Color _numberActiveColor;
        
        public Button Btn => _button;
        
        [ContextMenu("Set Usual")]
        public void SetUsual()
        {
            _image.color = _colorPassive;
            _highlightImage.enabled = false;
            _iconImage.sprite = _iconPassive;
            if (_number != null)
                _number.color = Color.white;
#if UNITY_EDITOR
            if(Application.isPlaying == false)
                UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        
        [ContextMenu("Set Active")]
        public void SetActive()
        {
            _image.color = _colorActive;
            _highlightImage.enabled = true;
            _iconImage.sprite = _iconActive;
            if (_number != null)
                _number.color = _numberActiveColor;
#if UNITY_EDITOR
            if(Application.isPlaying == false)
                UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void Scale()
        {
            _scaleEffect.Play();   
        }
        
    }
}