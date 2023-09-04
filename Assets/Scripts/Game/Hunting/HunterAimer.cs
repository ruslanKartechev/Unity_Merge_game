using System;
using System.Collections;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public class HunterAimer : MonoBehaviour
    {
        private const float UpOffset = 0.125f;

        [SerializeField] private AimVisualizer _visualizer;
        [SerializeField] private InputSettings _settings;

        private IHunter _hunter;
        private AimPath _aimPath;
        private Coroutine _inputTaking;
        // localToCamera transform
        private Vector3 _localOffset;
        private Transform _cameraTr;

        public void Activate()
        {
            _cameraTr = Camera.main.transform;
            _inputTaking = StartCoroutine(InputTaking());
        }

        public void Stop()
        {
            if(_inputTaking != null)
                StopCoroutine(_inputTaking);
        }
        
        public void SetHunter(IHunter hunter)
        {
            _hunter = hunter;
        }
        
        public void Jump()
        {
            _hunter.Jump(_aimPath);
            Stop();
        }

        public void StartAim()
        {
            _aimPath = new AimPath();
            // var tr = _hunter.GetTransform();
            // var forward = _cameraTr.forward;
            // forward.y = 0;
            // _localOffset = tr.InverseTransformVector(forward).normalized * 3;
            _localOffset = new Vector3(0, 0, 4);            
            _visualizer.Show(_aimPath);
            Calculate();
        }

        public void HideAim()
        {
            _visualizer.Hide();
        }

        public void Calculate()
        {
            var ht = _hunter.GetTransform();
            var start = ht.position;
            var end = start + _cameraTr.TransformVector(_localOffset);
            start.y = GetY(start);
            end.y = GetY(end);
            _aimPath.inflection = Vector3.Lerp(start, end, _settings.inflectionOffset) 
                                        + Vector3.up * _settings.inflectionUp;
            _aimPath.start = start;
            _aimPath.end = end;
            _visualizer.UpdatePath();
        }

        public void Move(Vector2 delta)
        {
            var localDelta = new Vector3(delta.x, 0, delta.y).normalized * _settings.Sensitivity;
            var offset= _localOffset +  localDelta;
            if (offset.magnitude > _settings.maxAimDistance)
                _localOffset = offset.normalized * _settings.maxAimDistance;
            else
                _localOffset = offset;
        }

        private IEnumerator InputTaking()
        {
            Vector2 oldPos = Input.mousePosition;
            Vector2 newPos;
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    oldPos = Input.mousePosition;
                    StartAim();    
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    HideAim();
                    Jump();
                }
                else if (Input.GetMouseButton(0))
                {
                    newPos = Input.mousePosition;
                    var delta = newPos - oldPos;
                    Move(delta);
                    Calculate();
                    oldPos = newPos;
                }
                yield return null;
            }
        }
        
        private float GetY(Vector3 pos)
        {
            if (Physics.Raycast(pos + Vector3.up * 25f, Vector3.down, out var hit, 100, _settings.groundMask))
                return hit.point.y + UpOffset;
            return pos.y + UpOffset;
        }
    }
}