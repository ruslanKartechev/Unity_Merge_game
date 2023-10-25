using System.Linq;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting
{
    public class BarbarianInAirBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _movable;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PreyAnimationKeys _animationKeys;
        [SerializeField] private OnTerrainPositionAdjuster _positionAdjuster;
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            var parent = transform.parent.parent;
            if (_movable == null)
                _movable = parent;
            if(_preyAnimator == null)
                _preyAnimator = HierarchyUtils.GetFromAllChildren<PreyAnimator>(parent).FirstOrDefault();
            if(_positionAdjuster == null)
                _positionAdjuster = HierarchyUtils.GetFromAllChildren<OnTerrainPositionAdjuster>(parent).FirstOrDefault();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
        public void OnGrabbed(Transform grabTo)
        {
            _positionAdjuster.enabled = false;
            _preyAnimator.PlayByTrigger(_animationKeys.GrabbedInAir);
            _movable.SetParent(grabTo);
        }
    }
}