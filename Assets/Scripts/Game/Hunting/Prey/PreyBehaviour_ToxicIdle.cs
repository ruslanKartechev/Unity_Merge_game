using System;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyBehaviour_ToxicIdle : MonoBehaviour, IPreyBehaviour
    {
        public event Action OnEnded;

        [SerializeField] private DropMode _dropMode;
        [SerializeField] private Mode _mode;
        [SerializeField] private ColdWeapon _weapon;
        [SerializeField] private ParticleSystem _fireParticles;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PreyAnimationKeys _animationKeys;

        private const float WeaponDropForce = 10f;
        
        public void Begin()
        {
            switch (_mode)
            {
                case Mode.None:
                    _preyAnimator.PlayByName("Idle");
                    _weapon.gameObject.SetActive(true);
                    if(_fireParticles != null)
                        _fireParticles.gameObject.SetActive(false);
                    break;
                case Mode.Random:
                    var random = UnityEngine.Random.Range(0f, 1f);
                    if (random >= .5)
                    {
                        Fire();
                    }
                    break;
                case Mode.ForceFire:
                    Fire();
                    break;
            }

            void Fire()
            {
                _preyAnimator.PlayByName(_animationKeys.BurnFireAnimation);
                _weapon.gameObject.SetActive(true);
                if (_fireParticles != null)
                {
                    _fireParticles.gameObject.SetActive(true);
                    _fireParticles.Play();
                }   
            }
        }

        public void Stop()
        {
            switch (_dropMode)
            {
                case DropMode.None:

                    break;
                case DropMode.Drop:
                    StopFire();
                    Drop();
                    break;
                case DropMode.StopFire:
                    if(_fireParticles != null)
                        _fireParticles.Stop();
                    break;
                case DropMode.DropRandom:
                    var random = UnityEngine.Random.Range(0f, 1f);
                    if (random >= .5)
                    {
                        StopFire();
                        Drop();
                    }
                    break;
            }
            void StopFire()
            {
                if (_fireParticles != null)
                    _fireParticles.Stop();
            }

            void Drop()
            {
                if(_weapon != null)
                    _weapon.Drop((transform.forward + Vector3.up) * WeaponDropForce);   
            }
        }

        public enum Mode
        {
            None,
            Random,
            ForceFire
        }

        public enum DropMode
        {
            None,
            StopFire,
            Drop,
            DropRandom
        }
    }
    
    
}