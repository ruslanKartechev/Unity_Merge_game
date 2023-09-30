using TMPro;
using UnityEngine;

namespace Game.UI.Map
{
    public class LevelUI : MonoBehaviour
    {
        [SerializeField] private int _level;
        [Space(5)]
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _levelTextPassed;
        [Space(10)]
        [SerializeField] private Transform _pointerPoint;
        [Space(10)]
        [SerializeField] private GameObject _activeBlock;
        [SerializeField] private GameObject _passiveBlock;

        public Vector3 PointerPosition => _pointerPoint.position;
        
        public void SetLevel(int level)
        {
            _level = level;
            _levelText.text = $"{_level}";
            _levelTextPassed.text = _levelText.text;
        }

        public void SetPassed()
        {
            _activeBlock.SetActive(true);
            _passiveBlock.SetActive(false);
        }

        public void SetLocked()
        {
            _activeBlock.SetActive(false);
            _passiveBlock.SetActive(true);
        }

        public void HideAll()
        {
            _activeBlock.SetActive(false);
            _passiveBlock.SetActive(false);
        }
    }
}