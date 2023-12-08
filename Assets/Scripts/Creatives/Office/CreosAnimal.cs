using System;
using System.Collections;
using Common;
using Common.Ragdoll;
using Creatives.Kong;
using Dreamteck.Splines;
using Game.Hunting;
using Game.Hunting.Hunters;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Creatives.Office
{
    public class CreosAnimal : MonoBehaviour, ICreosHunter
    {
        public event Action<ICreosHunter> OnDead;
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
        [SerializeField] private SplineFollower _follower;
        [SerializeField] private Transform _center;
        [SerializeField] private HunterMouth _hunterMouth;
        private Coroutine _input;
        private Coroutine _jumping;

        private void Start()
        {
            if(_autoStart)
                Run();
        }

        public void Run()
        {
            _animator.Play("Run");
            if (_creosSettings.useFollower)
            {
                _follower.followSpeed = _creosSettings.moveSpeed;
                _follower.Rebuild();
                _follower.enabled = true;                
            }
        }

        public void SetActive()
        {
            _aimer.Activate();
            StartInput();
        }
        
        public void Attack()
        {
            if (_creosSettings.useFollower)
                _follower.enabled = false;                
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
            _animator.Play(_creosSettings.attackKey);
            transform.parent = null;
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