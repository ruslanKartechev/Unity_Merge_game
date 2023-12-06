using System.Collections;
using System.Collections.Generic;
using Common;
using Creatives.Kong;
using UnityEngine;

namespace Creatives.Firemen
{
    public class JumpDownKong : MonoBehaviour
    {
        [SerializeField] private CreosAimer _aimer;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _jumpKey;
        [SerializeField] private string _landTrigger;
        [SerializeField] private float _landT;
        [SerializeField] private string _winTrigger;
        [SerializeField] private float _jumpTime;
        [SerializeField] private AnimationCurve _jumpAnimationCurve;
        [SerializeField] private Transform _movable;
        [SerializeField] private Transform _rotationTo;
        [SerializeField] private List<JumpDownKongListener> _listeners;
        private Coroutine _inputting;
        private Coroutine _working;

        private void Start()
        {
            _aimer.Activate();
            StartInput();
        }

        private void Jump()
        {
            _animator.Play(_jumpKey);
            StopInput();
            _working = StartCoroutine(Jumping());
        }

        private void Land()
        {
            Debug.Log("Landed");
            // _animator.SetTrigger(_winTrigger);
            foreach (var listener in _listeners)
            {
                if(listener == null)
                    continue;
                listener.OnLanded(_movable.position);
            }
        }
        
        private IEnumerator Jumping()
        {
            var elapsed = 0f;
            var time = _jumpTime;
            var t = elapsed / time;
            var path = _aimer.Path;
            var startRot = _movable.rotation;
            var endRot = _rotationTo.rotation;
            var landed = false;
            while (t <= 1f)
            {
                var pos = Bezier.GetPosition(path.start, path.inflection, path.end, t);
                _movable.position = pos;
                _movable.rotation = Quaternion.Lerp(startRot, endRot, t);
                t = elapsed / time;
                elapsed += Time.deltaTime * _jumpAnimationCurve.Evaluate(t);
                if (!landed)
                {
                    if (t >= _landT)
                    {
                        landed = true;
                        Debug.Log($"LAND KEY");
                        _animator.SetTrigger(_landTrigger);
                    }
                }
                yield return null;
            }

            _movable.position = path.end;
            Land();
        }


        private void StopInput()
        {
             if(_inputting != null)
                 StopCoroutine(_inputting);
        }

        private void StartInput()
        {
            StopInput();
            _inputting = StartCoroutine(InputTaking());
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
                    Jump();
                }
                yield return null;
            }
        }
    }
}