using System.Collections;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyListener_Barbarian : PreySurprisedListener, IHealthListener
    {
        [SerializeField] private float _runAnimationSpeed = 1f;
        [SerializeField] private float _rotDelay = 0.5f;
        [SerializeField] private PackUnitLocalMover _localMover;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PreyHealth _health;

        public override void OnInit()
        {
            _preyAnimator.SetRunAnimationSpeed(UnityEngine.Random.Range(_runAnimationSpeed * .9f, _runAnimationSpeed * 1.1f));
            // _health.AddListener(this);
        }

        public override void OnDead()
        {
            StopAllCoroutines();
        }

        public override void OnBeganRun()
        {
            _health.Show();
            _preyAnimator.Moving();
            _localMover.MoveToLocalPoint();
        }

        public override void OnSurprised()
        {
            _preyAnimator.Surprise();
            StartCoroutine(DelayedCall());
        }
        
        private IEnumerator DelayedCall()
        {
            yield return new WaitForSeconds(_rotDelay);
            _localMover.RotateToPoint();
        }

        public void OnHealthChange(float health, float maxHealth)
        {
            if (health <= 0)
            {
                PlayCritText();
                return;
            }
            PlayHitText();
        }

        private void PlayHitText()
        {
            var particles = Instantiate(GC.ParticlesRepository.GetParticles(EParticleType.TextHit), ParticlesPos(),
                Quaternion.identity);
            particles.Play();
        }

        private void PlayCritText()
        {
            var particles = Instantiate(GC.ParticlesRepository.GetParticles(EParticleType.TextCrit), ParticlesPos(),
                Quaternion.identity);
            particles.Play();
        }

        private Vector3 ParticlesPos()
        {
            return transform.position - transform.right * 1 + Vector3.up * 2;
        }
    }
}