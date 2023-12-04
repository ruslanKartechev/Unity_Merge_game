using System;
using System.Collections;
using Game.Hunting;
using Game.Hunting.Hunters;
using Game.Merging;
using UnityEngine;

namespace Creatives.Kong
{
    public class CreosAimer : MonoBehaviour
    {
        private const float UpOffset = 0.1f;
        
        public HunterAimSettings aimSettings;
        public Transform tr;
        public Vector3 startLocalPos;
        [SerializeField] private AimVisualizer _visualizer;
        [SerializeField] private InputSettings _settings;

        private AimPath _aimPath = new AimPath();
        private Coroutine _inputTaking;

        private Vector3 _localOffset;   // localToCamera transform
        private Transform _cameraTr;
        private Vector2 _inflectionUpLimits;

        private bool _startedAim;
        private Vector3 oldPos;
        private bool _isActive;

        public AimPath Path => _aimPath;
        
        public void Activate()
        {
            Stop();
            _cameraTr = Camera.main.transform;
            _visualizer.AimAtMask = aimSettings.AimMask;
            _visualizer.InflectionOffset = aimSettings.AimInflectionOffsetVisual;
            _inflectionUpLimits = aimSettings.AimInflectionUpLimits;
            _inputTaking = StartCoroutine(InputTaking());
            _isActive = true;
        }

        public void Stop()
        {
            if(_inputTaking != null)
                StopCoroutine(_inputTaking);
            _isActive = false;
        }
        
        public void StartAim()
        {
            _aimPath = new AimPath();
            var startLength = aimSettings.StartAimLength;
            _localOffset = startLocalPos;
            _visualizer.Show(_aimPath);
            CalculatePath(startLength);
        }
        
        public void OnDown()
        {
            if (!_isActive)
                return;
            _startedAim = true;   
            oldPos = Input.mousePosition;
            StartAim();
        }

        public void OnUp()
        {
            if (!_isActive || !_startedAim)
                return;
            _startedAim = false;
            HideAim();
        }

        public void HideAim()
        {
            _visualizer.Hide();
        }

        private void CalculatePath(float length)
        {
            var ht = tr;
            var start = ht.position;
            var end = start + _cameraTr.TransformVector(_localOffset);
            end.y = GetY(end);
            _aimPath.inflection = Vector3.Lerp(start, end, aimSettings.ArcInflectionLerp) 
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
            var length = _localOffset.magnitude;
            if (delta.magnitude < 2)
                return length;
            // delta.Normalize();
            delta /= 10;
            var localDelta = new Vector3( -1 * delta.x * _settings.SensitivityX(length), 0, 
                -1 * delta.y * _settings.SensitivityY(length)) * aimSettings.SensetivityMultipler;
            
            var offset= _localOffset +  localDelta;
            length = offset.magnitude;
            if (length > _settings.maxAimDistance)
                _localOffset = offset.normalized * _settings.maxAimDistance;
            else
                _localOffset = offset;
            return length;
        }
        

        
        private IEnumerator InputTaking()
        {
            while (true)
            {
                // Debug.Log($"Started: {_startedAim}. Mouse: {Input.GetMouseButton(0)}");
                if (_startedAim && Input.GetMouseButton(0))
                {
                    var newPos = Input.mousePosition;
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
            return 0f + UpOffset;
        }
    }
}