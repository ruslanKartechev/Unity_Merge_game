using System;
using System.Collections;
using System.Collections.Generic;
using Common.Ragdoll;
using Creatives.Kong;
using Dreamteck.Splines;
using Game.Hunting.Hunters;
using UnityEngine;

namespace Creatives.Office
{
    public class CreosPack : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _animals;
        [SerializeField] private SplineFollower _follower;
        private List<ICreosHunter> _hunters;
        private int _index;
        private void Awake()
        {
            _hunters = new List<ICreosHunter>(_animals.Count);
            foreach (var aa in _animals)
            {
                if(aa == null)
                    continue;
                var ia = aa.GetComponent<ICreosHunter>();
                if (ia != null)
                {
                    _hunters.Add(ia);
                    ia.OnDead += OnDead;
                }
            }
        }

        private void OnDead(ICreosHunter hunter)
        {
            hunter.OnDead -= OnDead;
            _index++;
            if (_index >= _hunters.Count)
                return;
            _hunters[_index].SetActive();
        }

        private void Start()
        {
            _follower.enabled = true;
            _follower.follow = true;
        }
        
        
    }
    public class CreosAnimal : MonoBehaviour, ICreosHunter
    {
        public event Action<ICreosHunter> OnDead;

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
                Begin();
        }

        public void Begin()
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
            StartInput();
        }
        
        public void Attack()
        {
            if (_creosSettings.useFollower)
                _follower.enabled = false;                
            StopInput();
            StopJump();
            _jumping = StartCoroutine(Jumping());
        }

        private IEnumerator Jumping()
        {
            var path = _aimer.Path;
            var elapsed = Time.deltaTime;
            var t = 0f;
            var time = ((path.end - path.inflection) + (path.inflection - path.start)).magnitude / _creosSettings.jumpSpeed;
            var curve = _creosSettings.jumpCurve;
            while (t <= 1)
            {
                var pos = path.GetPos(t);
                _movable.position = pos;
                t = elapsed / time;
                elapsed += Time.deltaTime * curve.Evaluate(t);
                Check();
                yield return null;
            }
            Fall();
        }

        private void Check()
        {
            var center = _center.position;
            var bite = Physics.OverlapSphere(center, _creosSettings.biteCastRad, _creosSettings.biteMask);
            if (bite.Length > 0)
            {
                Bite(bite[0]);
                return;
            }
            var fail = Physics.OverlapSphere(center, _creosSettings.failCastRad, _creosSettings.failMask);
            if (fail.Length > 0)
            {
                Debug.Log($"Collided fail with: {fail.Length} obejcts");
                BumpInto(fail);
            }
        }

        private void Bite(Collider coll)
        {
            StopJump();
            _ragdoll.Activate();
            _hunterMouth.BiteTo(coll.transform, coll.ClosestPoint(_center.position));
            OnDead?.Invoke(this);      
        }

        private void BumpInto(Collider[] targets)
        {
            StopJump();
            _ragdoll.Activate();
            OnDead?.Invoke(this);
        }
        
        private void Fall()
        {
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
                    Debug.Log("Start Aim");
                    _aimer.OnDown();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("Release");
                    _aimer.OnUp();
                    Attack();
                }
                yield return null;
            }
        }

    }
}