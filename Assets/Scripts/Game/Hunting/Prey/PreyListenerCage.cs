using System.Collections;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyListenerCage : PreyPackListener
    {
        [SerializeField] private float _startDelay;
        [SerializeField] private Animator _cageAnimator;
        [SerializeField] private string _cageAnimName;
        [SerializeField] private float _captiveRunDelay = 1f;
        [SerializeField] private Captive _captive;

        public override void OnAttacked()
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