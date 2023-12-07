using System;
using System.Collections;
using Common;
using Creatives.Kong;
using UnityEngine;

namespace Creatives.Rhino
{
    public class RunningRhino : MonoBehaviour
    {
        [SerializeField] private Animator _tankAnimator;
        [SerializeField] private RhinoCamera _rhinoCamera;
        [SerializeField] private Transform _runToPoint;
        [SerializeField] private ParticleSystem _onTargetParticles;
        [SerializeField] private ParticleSystem _attackParticles;
        [SerializeField] private ParticleSystem _dieParticles;
        [Space(10)] 
        [SerializeField] private bool _useAttackShake;
        [SerializeField] private CameraShakeArgs _attackShake;
        [SerializeField] private bool _useDieShake;
        [SerializeField] private CameraShakeArgs _dieShake;
        [Space(10)]
        [SerializeField] private AnimationCurve _speedCurve;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _runKey;
        [SerializeField] private string _attackKey;
        [SerializeField] private string _dieKey;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private CreosAimer _aimer;
        [SerializeField] private Transform _movable;
        [SerializeField] private string _attackCollideGOName;
        [SerializeField] private string _dieCollideGOName;
        [SerializeField] private Collider _trigger;
        private Coroutine _inputting;
        private Coroutine _working;

        private void Start()
        {
            _inputting = StartCoroutine(InputTaking());
            _aimer.Activate();
        }

        private void Run()
        {
            // _cameraAnimator?.Play("Move");
            _rhinoCamera.Follow();
            _animator.Play(_runKey);
            StopInput();
            StartCoroutine(Running(_runToPoint.position));
        }

        private void StopInput()
        {
            if(_inputting != null)
                StopCoroutine(_inputting);
        }


        private IEnumerator Running(Vector3 endPos)
        {
            var elapsed = 0f;
            var t = 0f;
            var startPos = _movable.position;
            var time = (endPos - startPos).magnitude / _moveSpeed;
            var vec = endPos - startPos;
            vec.y = 0f;
            var rotEnd = Quaternion.LookRotation(vec);
            while (t <= 1f)
            {
                var pos = Vector3.Lerp(startPos, endPos, t);
                _movable.position = pos;
                _movable.rotation = Quaternion.Lerp(_movable.rotation, rotEnd, .3f);
                t = elapsed / time;
                elapsed += Time.deltaTime * _speedCurve.Evaluate(t);
                yield return null;
            }
            _movable.position = endPos;
            Die();
        }

        private void Attack(GameObject go)
        {
            _animator.Play(_attackKey);
            if (go.TryGetComponent<KongPushTarget>(out var target))
            {
                target.Push();
                if(_useAttackShake)
                    _cameraShaker.Play(_attackShake);
            }
            if (_attackParticles != null)
            {
                var instsance = Instantiate(_attackParticles, _attackParticles.transform.position, Quaternion.identity);
                instsance.Play();
            }
        }
        
        private void Die()
        {
            _tankAnimator.Play("Push");
            _rhinoCamera.Bump();
            if(_useDieShake)
                _cameraShaker.Play(_dieShake);
            _trigger.enabled = false;
            _animator.SetTrigger(_dieKey);
            if (_dieParticles != null)
            {
                var instsance = Instantiate(_dieParticles, _attackParticles.transform.position, Quaternion.identity);
                instsance.Play();
            }
            if(_onTargetParticles != null)
                _onTargetParticles.Play();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log($"name: {other.gameObject.name}");
            // if (other.gameObject.name.Contains(_dieCollideGOName))
            // {
            //     Debug.Log($"!! [Rhino] Die");
            //     Die();
            //     return;
            // }
            if (other.gameObject.name.Contains(_attackCollideGOName))
            {
                  Debug.Log($"!! [Rhino] attack");
                  Attack(other.gameObject);
            } 
       
        }

        private IEnumerator InputTaking()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _aimer.OnDown();
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    _aimer.OnUp();
                    Run();
                }
                yield return null;
            }
        }
    }
}