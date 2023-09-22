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
        private Vector2 _inflectionUpLimits;
        

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
            _visualizer.InflectionOffset = _hunter.AimInflectionOffsetVisual();
            _inflectionUpLimits = _hunter.AimInflectionUpLimits();
        }
        
        public void Jump()
        {
            _hunter.Jump(_aimPath);
            Stop();
        }

        public void StartAim()
        {
            _aimPath = new AimPath();
            var startLength = 4;
            _localOffset = new Vector3(0, 0, startLength);            
            _visualizer.Show(_aimPath);
            CalculatePath(startLength);
        }

        public void HideAim()
        {
            _visualizer.Hide();
        }

        private void CalculatePath(float length)
        {
            var ht = _hunter.GetTransform();
            var start = ht.position;
            var end = start + _cameraTr.TransformVector(_localOffset);
            end.y = GetY(end);
            _aimPath.inflection = Vector3.Lerp(start, end, _settings.inflectionOffset) 
                                        + Vector3.up * VerticalOffset(length);
            _aimPath.start = start;
            _aimPath.end = end;
            _visualizer.UpdatePath();
        }
        
        private float VerticalOffset(float distance) =>
            Mathf.Lerp(_inflectionUpLimits.x, _inflectionUpLimits.y, 
                Mathf.InverseLerp(0, _settings.maxAimDistance, distance));
        
        /// <summary>
        /// Returns Length of the line in XZ plane
        /// </summary>
        private float MoveAimAndGetLength(Vector2 delta)
        {
            var localDelta = -new Vector3(delta.x, 0, delta.y).normalized * _settings.Sensitivity;
            var offset= _localOffset +  localDelta;
            var length = offset.magnitude;
            if (length > _settings.maxAimDistance)
                _localOffset = offset.normalized * _settings.maxAimDistance;
            else
                _localOffset = offset;
            return length;
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
                    var length = MoveAimAndGetLength(delta);
                    CalculatePath(length);
                    oldPos = newPos;
                }
                yield return null;
            }
        }
        
        private float GetY(Vector3 pos)
        {
            if (Physics.Raycast(pos + Vector3.up * 50f, Vector3.down, out var hit, 100, _settings.groundMask))
                return hit.point.y + UpOffset;
            return pos.y + UpOffset;
        }
    }
}