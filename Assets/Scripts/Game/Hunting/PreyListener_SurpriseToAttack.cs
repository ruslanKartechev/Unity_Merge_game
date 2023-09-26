using System.Collections;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyListener_SurpriseToAttack : PreySurprisedListener
    {
        [SerializeField] private float _runAnimationSpeed = 1f;
        [Space(10)]
        [SerializeField] private float _rotTime = 1f;
        [SerializeField] private float _rotDelay = 0.5f;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PreyHealth _health;
        [SerializeField] private Transform _rotatable;

        public override void OnStarted()
        {
            _preyAnimator.SetRunAnimationSpeed(UnityEngine.Random.Range(_runAnimationSpeed * .9f, _runAnimationSpeed * 1.1f));
        }
        
        public override void OnDead()
        { }

        public override void OnBeganRun()
        {
            _health.Show();
            _preyAnimator.Moving();
        }

        public override void OnSurprised()
        {
            _preyAnimator.Surprise();
            RotateToLocal();
        }

        public void RotateToLocal()
        {
            StartCoroutine(RotatingToLocal());
        }
        
        private IEnumerator RotatingToLocal()
        {
            yield return new WaitForSeconds(_rotDelay);
            var elapsed = 0f;
            var rot1 = _rotatable.localRotation;
            var rot2 = Quaternion.identity;
            var time = _rotTime;
            while (elapsed <= time)
            {
                _rotatable.localRotation = Quaternion.Lerp(rot1, rot2, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _rotatable.localRotation = rot2;
        }
    }
}