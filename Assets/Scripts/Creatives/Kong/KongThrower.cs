using System;
using System.Collections;
using UnityEngine;

namespace Creatives.Kong
{
    public class KongThrower : MonoBehaviour
    {
        [SerializeField] private float _throwForce;
        [SerializeField] private float _upForce;
        [SerializeField] private Throwable _throwable;
        [SerializeField] private ThrowableHuman _throwableHuman;
        [SerializeField] private Transform _grabParent;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _grabKey;
        [SerializeField] private string _throwKey;
        [SerializeField] private string _prepareThrowKey;
        [SerializeField] private CreosAimer _aimer;
        [SerializeField] private KongThrowerEventCatcher _eventCatcher;
        [SerializeField] private Transform _target;
        [SerializeField] private float _moveTime;
        private Coroutine _working;
        
        private void Start()
        {
            _eventCatcher.OnGrabEvent += OnGrab;
            _eventCatcher.OnThrowEvent += OnThrow;
            _aimer.Activate();
            Stop();
            _animator.Play("Run");
            _working = StartCoroutine(RunningToPoint(Grab));
        }

        private void Grab()
        {
            _animator.SetTrigger(_grabKey);
        }
        
        private void OnThrow()
        {
            Debug.Log("On Throw");
            var path = _aimer.Path;
            var dir = path.end - path.start;
            dir.y = 0f;
            var force = (dir).normalized * _throwForce;
            force += Vector3.up * _upForce;
            if (_throwable != null)
                _throwable.Throw(force);
            else if (_throwableHuman != null)
                _throwableHuman.Throw(force);
        }

        private void OnGrab()
        {
            Debug.Log("On Grabbed");
            if (_throwable != null)
            {
                _throwable.Grab(_grabParent);
            }
            else if (_throwableHuman != null)
            {
                _throwableHuman.Grab(_grabParent);
            }
            
            Stop();
            _working = StartCoroutine(Input());
        }

        private void ThrowTarget()
        {
            _animator.SetTrigger(_throwKey);
        }

        private void Stop()
        {
            if(_working != null)
                StopCoroutine(_working);
        }

        private void Prepare()
        {
            if(_prepareThrowKey != "")
                _animator.Play(_prepareThrowKey);
        }
        
        private IEnumerator Input()
        {
            while (true)
            {
                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Start Aim");
                    Prepare();
                    _aimer.OnDown();
                }
                else if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    Debug.Log("Release");
                    _aimer.OnUp();
                    ThrowTarget();
                }
                yield return null;
            }
        }

        private IEnumerator RunningToPoint(Action onDone)
        {
            var elapsed = 0f;
            var time = _moveTime;
            var tr = transform;
            var pos1 = tr.position;
            var pos2 = _target.position;
            var rot1 = tr.rotation;
            var rot2 = _target.rotation;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                tr.position = Vector3.Lerp(pos1, pos2, t);
                tr.rotation = Quaternion.Lerp(rot1, rot2, t);
                elapsed += Time.deltaTime;
                yield return null;
            }
            tr.SetPositionAndRotation(_target.position, _target.rotation);
            onDone.Invoke();
        }


        private IEnumerator Delayed(float time, Action onDone)
        {
            yield return new WaitForSeconds(time);
            onDone.Invoke();
        }
    }
}