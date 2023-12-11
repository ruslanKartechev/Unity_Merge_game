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
        [SerializeField] private float _jumpTime;
        [SerializeField] private string _winTrigger;
        [Space(10)]
        [SerializeField] private string _turnTrigger;
        [SerializeField] private float _turnDelay;
        [SerializeField] private float _turnTime;
        [SerializeField] private float _turnAngle;
        [SerializeField] private AnimationCurve _turnCurve;
        [Space(10)]
        [SerializeField] private AnimationCurve _jumpAnimationCurve;
        [SerializeField] private Transform _movable;
        [SerializeField] private Transform _rotationTo;
        [SerializeField] private JumpKongCamera _camera;
        
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
            if(_camera != null)
                _camera.MoveToP2();
        }

        private void Land()
        {
            foreach (var listener in _listeners)
            {
                if(listener == null)
                    continue;
                listener.OnLanded(_movable.position);
            }

            StartCoroutine(DelayedTurn());
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

        private IEnumerator DelayedTurn()
        {
            yield return new WaitForSeconds(_turnDelay);
            _animator.SetTrigger(_turnTrigger);
            var elapsed = 0f;
            var time = _turnTime;
            var curve = _turnCurve;
            var t = 0f;
            var tr = _movable;
            var angles = tr.localEulerAngles;
            var a1 = angles.y;
            var a2 = _turnAngle;
            while (t <= 1f)
            {
                angles.y = Mathf.Lerp(a1, a2, t);
                tr.localEulerAngles = angles;
                t = elapsed / time;
                elapsed += Time.deltaTime * curve.Evaluate(t);
                yield return null;
            }
            _animator.SetTrigger(_winTrigger);
        }
    }
}