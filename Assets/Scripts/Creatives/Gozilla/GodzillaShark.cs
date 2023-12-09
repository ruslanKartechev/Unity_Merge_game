using System;
using UnityEngine;
using System.Collections;
using Common;
using Common.Ragdoll;
using Creatives.Kong;
using Creatives.Office;
using Game.Hunting;
using Game.Hunting.Hunters;
using Game.Hunting.Prey.Interfaces;

namespace Creatives.Gozilla
{
    public class GodzillaShark : MonoBehaviour, ICreosHunter
    {
        public event Action<ICreosHunter> OnDead;
        [SerializeField] private bool _shakeOnObstacleHit;
        [SerializeField] private CameraShakeArgs _obstacleHitShakeArgs;
        [Header("Enemy jump to")]
        [SerializeField] private bool _jumpToEnemy;
        [SerializeField] private GodzillaHazmat _enemy;
        [SerializeField] private DiveCurve _diveCurve;
        [Header("Custom target")]
        [SerializeField] private bool _useCustomTarget;
        [SerializeField] private Transform _jumpToTarget;
        [SerializeField] private bool _checkEnemies = true;
        [SerializeField] private bool _hitOnEnd = false;
        [Space(12)]
        [SerializeField] private float _damage = 10;
        [SerializeField] private float _lerpRotSpeed = .33f;
        [SerializeField] private bool _autoStart;
        [SerializeField] private Transform _movable;
        [SerializeField] private CreosAimer _aimer;
        [SerializeField] private Animator _animator;
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private CreosSettings _creosSettings;
        [SerializeField] private Transform _center;
        [SerializeField] private HunterMouth _hunterMouth;
        [Space(12)] 
        [SerializeField] private ParticleSystem _furnitureParticles;
        [SerializeField] private ParticleSystem _failParticles;
        private Coroutine _input;
        private Coroutine _jumping;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            //UnityEditor.EditorUtility.SetDirty(this);
            
        }
#endif

        private void Start()
        {
            if(_autoStart)
                Run();
        }
        
                
        public void Activate(bool now)
        {
            
        }

        public void Run()
        {
            Run();
            _animator.Play("Run");
        }

        public void SetActive()
        {
            _aimer.Activate();
            StartInput();
        }
        
        public void Attack()
        {
            StopInput();
            StopJump();
            if (_useCustomTarget && _jumpToTarget != null)
            {
                _jumping = StartCoroutine(JumpingCustomTarget());
            }
            else
            {
                _jumping = StartCoroutine(Jumping());
            }
        }

        private IEnumerator JumpingCustomTarget()
        {
            transform.parent = null;
            _animator.Play(_creosSettings.attackKey);
            var path = _aimer.Path;
            var elapsed = Time.deltaTime;
            var t = 0f;
            var time = ((_jumpToTarget.position - path.inflection) + (path.inflection - path.start)).magnitude / _creosSettings.jumpSpeed;
            var curve = _creosSettings.jumpCurve;
            while (t <= 1)
            {
                var pos = Bezier.GetPosition(path.start, path.inflection, _jumpToTarget.position, t);
                var rotVec = _jumpToTarget.position - _movable.position;
                rotVec.y = 0f;
                var rot2 = Quaternion.LookRotation(rotVec);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, rot2, _lerpRotSpeed);
                _movable.position = pos;
                t = elapsed / time;
                elapsed += Time.deltaTime * curve.Evaluate(t);
                if(_checkEnemies)
                    Check();
                yield return null;
            }
            if (_hitOnEnd)
            {
                HitFail();
                yield break;
            }
            if (_checkEnemies)
            {
                if(Check())
                    yield break;           
            }
            Fall();
        }
        
        private IEnumerator Jumping()
        {
            _animator.Play(_creosSettings.attackKey);
            transform.parent = null;
            var path = _aimer.Path;
            var elapsed = Time.deltaTime;
            var t = 0f;
            var time = ((path.end - path.inflection) + (path.inflection - path.start)).magnitude / _creosSettings.jumpSpeed;
            var curve = _creosSettings.jumpCurve;
            var rotVec = path.end - _movable.position;
            rotVec.y = 0f;
            var rot2 = Quaternion.LookRotation(rotVec);
            while (t <= 1)
            {
                var pos = path.GetPos(t);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, rot2, _lerpRotSpeed);
                _movable.position = pos;
                t = elapsed / time;
                elapsed += Time.deltaTime * curve.Evaluate(t);
                Check();
                yield return null;
            }
            Fall();
        }

        private bool Check()
        {
            var center = _center.position;
            var bite = Physics.OverlapSphere(center, _creosSettings.biteCastRad, _creosSettings.biteMask);
            // Debug.Log($"Bite length: {bite.Length}");
            if (bite.Length > 0)
            {
                // Debug.Log($"DID FIND A TARGET ********");
                // var ind = 1;
                // foreach (var cl in bite)
                // {
                //     Debug.Log($"go {ind} : {cl.gameObject.name},");
                //     ind++;
                // }
                return Bite(bite[0]);
            }
            var fail = Physics.OverlapSphere(center, _creosSettings.failCastRad, _creosSettings.failMask);
            if (fail.Length > 0)
            {
                Debug.Log($"Collided fail with: {fail.Length} obejcts");
                BumpInto(fail);
                return true;
            }
            return false;
        }

        private bool Bite(Collider coll)
        {
            var target = coll.GetComponentInParent<IPredatorTarget>();
            if (target == null || target.IsAlive() == false)
                return false;
            
            target.Damage(new DamageArgs(_damage, _center.position, (_aimer.Path.end - _aimer.Path.start).normalized));
            _animator.enabled = false;
            StopJump();
            _ragdoll.Activate();
            _hunterMouth.BiteTo(coll.transform, coll.ClosestPoint(_center.position));
            OnDead?.Invoke(this);
            return true;
        }

        private void BumpInto(Collider[] tt)
        {
            StopJump();

            var center = tt[0].transform.position;
            var targets = Physics.OverlapSphere(center, _creosSettings.areaCastRad);
            Debug.Log($"Bumped into: {targets.Length}");
            var didHit = false;
            foreach (var tr in targets)
            {
                tr.gameObject.layer = 0;
                var rb = tr.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.isKinematic = false;
                    var force = (tr.transform.position - _center.position).normalized;
                    force *= _creosSettings.pushForce;
                    force += Vector3.up * _creosSettings.pushForceUp;
                    rb.AddForce(force, ForceMode.Impulse);
                    didHit = true;
                }
            }
            if (didHit)
            {
                if (_shakeOnObstacleHit)
                {
                    var shaker = FindObjectOfType<CameraShaker>();
                    shaker?.Play(_obstacleHitShakeArgs);
                }
                if (_furnitureParticles != null)
                {
                    _furnitureParticles.gameObject.SetActive(true);
                    _furnitureParticles.transform.parent = null;
                    _furnitureParticles.Play();            
                }
            }

            _animator.enabled = false;
            _ragdoll.ActivateAndPush(transform.forward * _creosSettings.bumpRagdolForce);
            OnDead?.Invoke(this);
        }
        
        private void Fall()
        {
            _animator.enabled = false;
            StopJump();
            _ragdoll.Activate();
            OnDead.Invoke(this);
        }

        private void HitFail()
        {
            _animator.enabled = false;
            StopJump();
            _ragdoll.Activate();
            // foreach (var listener in _listeners)
            // {
            //     if(listener ==null)
            //         continue;
            //     if (listener.TryGetComponent<ICreosAnimalListener>(out var ss))
            //     {
            //         ss.OnFailHit();
            //     }
            // }

            if (_failParticles != null)
            {
                _failParticles.gameObject.SetActive(true);
                _failParticles.transform.parent = null;
                _failParticles.Play();
            }
            OnDead.Invoke(this);
        }
        
        private void StopJump()
        {
            if(_jumping != null)
                StopCoroutine(_jumping);
        }
        
        private void StartInput()
        {
            StopInput();
            _input = StartCoroutine(Inputting());
        }
        
        private void StopInput()
        {
            if(_input != null)
                StopCoroutine(_input);
        }
        
        private IEnumerator Inputting()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // Debug.Log("Start Aim");
                    _aimer.OnDown();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    // Debug.Log("Release");
                    _aimer.OnUp();
                    Attack();
                }
                yield return null;
            }
        }
    }
}