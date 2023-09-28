using UnityEngine;
using Common;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(InputSettings), fileName = nameof(InputSettings), order = 0)]
    public class InputSettings : ScriptableObject
    {
        [SerializeField] private LimitedValue _sensitivityX;
        [SerializeField] private LimitedValue _sensitivityY;
        public LayerMask mergingMask;
        public LayerMask groundMask;
        public float inflectionOffset = 0.66f;
        public float maxAimDistance = 6;
        public float draggingUpOffset = .3f;

        
#if UNITY_EDITOR
        private const float EditorSensMultiplier = 1f * .5f;
        
#endif

        public float SensitivityX(float distance)
        {
            var val = _sensitivityX.GetValue(distance);
            // Debug.Log($"Sensitivity x: {val}, distance: {distance}");
#if UNITY_EDITOR
            val *= EditorSensMultiplier;
#endif
            return val;
        }
        
        public float SensitivityY(float distance)
        {
            var val = _sensitivityY.GetValue(distance);
#if UNITY_EDITOR
            val *= EditorSensMultiplier;
#endif
            return val;
        }

        public void SetSensitivityX(float value)
        {
#if UNITY_EDITOR
            value /= EditorSensMultiplier;
#endif
            _sensitivityX.SetMaxValue(value);            
        }
        
        public void SetSensitivityY(float value)
        {
#if UNITY_EDITOR
            value /= EditorSensMultiplier;
#endif
            _sensitivityY.SetMaxValue(value);            
        }

        public float GetMaxSensX() => _sensitivityX.MaxValue;
        public float GetMinSensX() => _sensitivityX.MinValue;

        public float GetMaxSensY() => _sensitivityY.MaxValue;
        public float GetMinSensY() => _sensitivityY.MinValue;
        

    }

}