using System;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyBehaviour_ToxicIdle : MonoBehaviour, IPreyBehaviour
    {
        [SerializeField] private bool _forceBurn;
        [SerializeField] private ColdWeapon _weapon;
        [SerializeField] private ParticleSystem _fireParticles;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PreyAnimationKeys _animationKeys;
        [SerializeField] private PreyRandomWeaponPicker _randomWeaponPicker;

        private const float WeaponDropForce = 10f;
        
        public void Begin()
        {
            var random = UnityEngine.Random.Range(0f, 1f);
            if (random >= .5 || _forceBurn)
            {
                // active burning
                _preyAnimator.PlayByName(_animationKeys.BurnFireAnimation);
                _weapon.gameObject.SetActive(true);
                if (_fireParticles != null)
                {
                    _fireParticles.gameObject.SetActive(true);
                    _fireParticles.Play();
                }
            }
            else
            {
                _preyAnimator.PlayByName("Idle");
                _weapon.gameObject.SetActive(true);
                if(_fireParticles != null)
                    _fireParticles.gameObject.SetActive(false);
            }
        }

        public void Stop()
        {
            var random = UnityEngine.Random.Range(0f, 1f);
            if(_fireParticles != null)
                _fireParticles.Stop();
            if (random >= .5)
            {
                if(_weapon != null)
                    _weapon.Drop((transform.forward + Vector3.up) * WeaponDropForce);
            }
        }

        public event Action OnEnded;
    }
    
    
}