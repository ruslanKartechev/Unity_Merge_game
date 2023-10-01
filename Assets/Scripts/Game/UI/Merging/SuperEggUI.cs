using System.Collections;
using DG.Tweening;
using Game.Merging;
using TMPro;
using UnityEngine;

namespace Game.UI.Merging
{
    public class SuperEggUI : MonoBehaviour, ISuperEggUI
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _labelText;
        [SerializeField] private GameObject _block;
        [SerializeField] private float _moveDownTime = 0.5f;
        [SerializeField] private Vector2 _downAnchorPos;
        
        [SerializeField] private RectTransform _rect;
        private SuperEgg _egg;
        
        public void Show(SuperEgg egg)
        {
            _egg = egg;
            _timerText.text = egg.TimeLeft.TimeAsString;
            _labelText.text = egg.Label;
            _block.SetActive(true);
            StartCoroutine(Working());
        }

        public void MoveDown()
        {
            _rect.DOAnchorPos(_downAnchorPos, _moveDownTime);
        }

        public void Hide()
        {
            _block.SetActive(false);
        }

        public void ShowLabel()
        {
            _labelText.enabled = true;
        }

        public void HideLabel()
        {
            _labelText.enabled = false;
        }

        private IEnumerator Working()
        {
            _labelText.text = _egg.Label;
            while (true)
            {
                _timerText.text = _egg.TimeLeft.TimeAsString;
                yield return null;
            }
        }   
    }
}