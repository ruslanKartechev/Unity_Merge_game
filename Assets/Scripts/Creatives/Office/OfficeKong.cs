using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Ragdoll;
using Creatives.Kong;
using Dreamteck.Splines;
using Game.Hunting;
using UnityEngine;

namespace Creatives.Office
{
    public class OfficeKong : MonoBehaviour
    {
        [SerializeField] private float _doorOpenDelay;
        [SerializeField] private float _runDelay;
        [SerializeField] private float _cameraTransitionDelay;
        [Header("Acceleration at start")]
        [SerializeField] private bool _accelerateOnStart;
        [SerializeField] private float _accelerationTime;
        [SerializeField] private float _startSpeed;
        [SerializeField] private float _mainSpeed;
        [Header("Attack config")] 
        [SerializeField] private string _attackKey;
        [SerializeField] private float _damage;
        [SerializeField] private CreosSettings _creosSettings;
        [SerializeField] private Transform _center;
        [Space(10)] 
        [SerializeField] private Elevator _elevator;
        [SerializeField] private OfficeKongCamera _camera;
        [SerializeField] private SplineFollower _splineFollower;
        [SerializeField] private Transform _movable;
        [SerializeField] private CreosAimer _aimer;
        [SerializeField] private IRagdoll _ragdoll;
        [Space(10)]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _startAnim;
        [SerializeField] private string _runAnim;
        [Space(10)]
        [SerializeField] private bool _shakeOnObstacleHit;
        [SerializeField] private CameraShakeArgs _obstacleHitShakeArgs;
        [Space(10)] 
        [SerializeField] private ParticleSystem _furnitureParticles;
        [SerializeField] private ParticleSystem _failParticles;

        private Coroutine _accelerating;
        private Coroutine _input;
        private Coroutine _jumping;
        
        private void Start()
        {
            StartCoroutine(Starting());
        }

        private IEnumerator Starting()
        {
            _animator.Play(_startAnim);
            _camera.StartFollowP1();
            yield return new WaitForSeconds(_doorOpenDelay);
            _elevator.Open();
            yield return new WaitForSeconds(_runDelay);
            _animator.SetTrigger(_runAnim);
            SplineHelper.SetOffset(_splineFollower);
            _splineFollower.followSpeed = 0f;
            _splineFollower.enabled = _splineFollower.follow = true;
            AllowAim();
            _input = StartCoroutine(Inputting());
            AccelerateOnStart();
            yield return DelayedCameraTransition();
        }
        
        private void StopAcceleration()
        {
            if(_accelerating != null)
                StopCoroutine(_accelerating);   
        }
        
        private void AccelerateOnStart()
        {
         
            if (_accelerateOnStart)
            {
                _accelerating = StartCoroutine(Accelerating(_startSpeed, _mainSpeed, _accelerationTime));
            }
            else
            {
                _splineFollower.followSpeed = _mainSpeed;
            }
        }

        private IEnumerator DelayedCameraTransition()
        {
            yield return new WaitForSeconds(_cameraTransitionDelay);
            _camera.TransitionToP2();
        }

        private IEnumerator Accelerating(float speed1, float speed2, float duration)
        {
            var elapsed = Time.deltaTime;
            var t = 0f;
            var time = duration;
            while (t <= 1f)
            {
                var s = Mathf.Lerp(speed1, speed2, t);
                _splineFollower.followSpeed = s;
                t = elapsed / time;
                elapsed += Time.deltaTime;
                yield return null;
            }
            _splineFollower.followSpeed = speed2;
        }
        
        private void AllowAim()
        {
            _aimer.enabled = true;
            _aimer.Activate();
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
        
        private void Attack()
        {
            Debug.Log("[OfficeKong] Attack started");
            _camera.PreserveY = true;
            StopInput();
            StopAcceleration();
            _splineFollower.follow = false;
            _jumping = StartCoroutine(Jumping());
        }
        
        private void StopJump()
        {
            if(_jumping != null)
                StopCoroutine(_jumping);
        }
        

        private IEnumerator Jumping()
        {
            _animator.SetTrigger(_attackKey);
            transform.parent = null;
            var path = _aimer.Path;
            var elapsed = Time.deltaTime;
            var t = 0f;
            var time = ((path.end - path.inflection) + (path.inflection - path.start)).magnitude / _creosSettings.jumpSpeed;
            // var curve = _creosSettings.jumpCurve;
            var rotVec = path.end - _movable.position;
            rotVec.y = 0f;
            var rot2 = Quaternion.LookRotation(rotVec);
            while (t <= 1)
            {
                var pos = path.GetPos(t);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, rot2, .33f);
                _movable.position = pos;
                t = elapsed / time;
                elapsed += Time.deltaTime;
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
            var target = coll.GetComponentInParent<IDamageable>();
            if (target == null || target.IsAlive() == false)
                return false;
            target.Damage(new DamageArgs(_damage, _center.position, (_aimer.Path.end - _aimer.Path.start).normalized));
            _animator.enabled = false;
            StopJump();
            _ragdoll.Activate();
            Fall();
            return true;
        }

        private void BumpInto(Collider[] tt)
        {
            StopJump();

            var center = tt[0].transform.position;
            var targets = Physics.OverlapSphere(center, _creosSettings.areaCastRad);
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
            RaiseDead();
        }
        
        private void Fall()
        {
            _animator.enabled = false;
            StopJump();
            _ragdoll.Activate();
            RaiseDead();
        }

        private void RaiseDead()
        {
            // OnDead.Invoke(this);
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
            RaiseDead();
        }
    }
}