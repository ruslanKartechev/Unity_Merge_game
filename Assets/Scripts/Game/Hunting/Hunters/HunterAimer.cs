using System.Collections;
using Common;
using Game.Hunting.Hunters.Interfaces;
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
        private AimPath _aimPath = new AimPath();
        private Coroutine _inputTaking;

        private Vector3 _localOffset;   // localToCamera transform
        private Transform _cameraTr;
        private Vector2 _inflectionUpLimits;

        private bool _startedAim;
        private Vector3 oldPos;
        private bool _isActive;
        
        private ProperButton _button; 
        public ProperButton InputButton
        {
            get => _button;
            set
            {
                _button = value;
                _button.OnDown += OnDown;
                _button.OnUp += OnUp;
            } 
        }
        
        public void Activate()
        {
            Stop();
            _cameraTr = Camera.main.transform;
            _inputTaking = StartCoroutine(InputTaking());
            _isActive = true;
        }

        public void Stop()
        {
            if(_inputTaking != null)
                StopCoroutine(_inputTaking);
            _isActive = false;
        }
        
        public void SetHunter(IHunter hunter)
        {
            _hunter = hunter;
            _visualizer.InflectionOffset = _hunter.AimSettings.AimInflectionOffsetVisual;
            _inflectionUpLimits = _hunter.AimSettings.AimInflectionUpLimits;
            
        }
        
        public void Jump()
        {
            _hunter.Jump(_aimPath);
            Stop();
        }

        public void StartAim()
        {
            _aimPath = new AimPath();
            var startLength = _hunter.AimSettings.StartAimLength;
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
            _aimPath.inflection = Vector3.Lerp(start, end, _hunter.AimSettings.ArcInflectionLerp) 
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
                -1 * delta.y * _settings.SensitivityY(length)) * _hunter.AimSettings.SensetivityMultipler;
            
            var offset= _localOffset +  localDelta;
            length = offset.magnitude;
            if (length > _settings.maxAimDistance)
                _localOffset = offset.normalized * _settings.maxAimDistance;
            else
                _localOffset = offset;
            return length;
        }
        
        private void OnDown()
        {
            if (!_isActive)
                return;
            oldPos = Input.mousePosition;
            StartAim();
            _startedAim = true;   
        }

        private void OnUp()
        {
            if (!_isActive || !_startedAim)
                return;
            HideAim();
            Jump();
            _startedAim = false;
        }
        
        private IEnumerator InputTaking()
        {
            var inp = GC.Input;
            while (true)
            {
                if (_startedAim && inp.IsPressed())
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
            return pos.y + UpOffset;
        }
    }
}