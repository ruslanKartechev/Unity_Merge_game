using System;
using System.Collections;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class DeadColorPainter : MonoBehaviour
    {
        [SerializeField] private Color _deadColor;
        [SerializeField] private float _delayBeforeDead;
        [SerializeField] private float _deadFadeTime;
        [SerializeField] private RendererColorer _colorer;

        public void PaintDead()
        {
            StartCoroutine(DeadColorSettings());
        }
        

        private IEnumerator DeadColorSettings()
        {
            yield return new WaitForSeconds(_delayBeforeDead);
            _colorer.FadeToColor(_deadColor, _deadFadeTime);
        }
    }
}