using System;
using System.Collections;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerCage : PreySurprisedListener
    {
        [SerializeField] private float _startDelay;
        [SerializeField] private Animator _cageAnimator;
        [SerializeField] private string _cageAnimName;
        [SerializeField] private float _captiveRunDelay = 1f;
        [SerializeField] private Captive _captive;

        private void Start()
        {
            transform.SetParent(null);
        }

        public override void OnStarted()
        { }

        public override void OnDead()
        { }

        public override void OnBeganRun()
        { }

        public override void OnSurprised()
        {
            StartCoroutine(Working());
        }

        private IEnumerator Working()
        {
            yield return new WaitForSeconds(_startDelay);
            _cageAnimator.Play(_cageAnimName);
            yield return new WaitForSeconds(_captiveRunDelay);
            _captive.RunAway();
        }
    }
}