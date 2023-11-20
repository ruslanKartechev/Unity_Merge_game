using System;
using System.Collections;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Map
{
    public class MapLevelElement : MonoBehaviour
    {
        [SerializeField] private Image _backRend;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _aditionalIcon;

        public void SetNumber(int level)
        {
            _text.text = $"{level}";
        }

        public void SetPassed(MapLevelSprites sprites)
        {
            _backRend.sprite = sprites.Passed;
        }

        public void SetCurrent(MapLevelSprites sprites)
        {
            _backRend.sprite = sprites.Current;
        }

        public void SetFuture(MapLevelSprites sprites)
        {
            _backRend.sprite = sprites.Future;
        }

        public void SetAdditionalIcon(Sprite sprite)
        {
            _aditionalIcon.sprite = sprite;
            _aditionalIcon.enabled = true;
        }

        public IEnumerator Animating(MapLevelSprites sprites, float duration, Action onSwitch)
        {
            var elapsed = 0f;
            bool switched = false;
            var t = 0f;
            while (t <= 1)
            {
                t = elapsed / duration;
                var scale = Bezier.GetValue(.95f, 1.5f, 1f,  t);
                transform.localScale = Vector3.one * scale;
                if (!switched)
                {
                    if (t >= .5f)
                    {
                        switched = true;
                        SetPassed(sprites);
                        onSwitch.Invoke();
                    }
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localScale = Vector3.one;
        }
        
    }
}