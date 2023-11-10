using System;
using System.Linq;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyBehaviour_OilBarrelSurprised : MonoBehaviour, IPreyBehaviour
    {
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PreyAnimationKeys _animationKeys;
        [SerializeField] private BarrelThrowable _barrel;
        [SerializeField] private PackUnitLocalMover _localMover;

        
#if UNITY_EDITOR
        public bool _getRefs;
        private void OnValidate()
        {
            if (Application.isPlaying || !_getRefs)
                return;
            var parent = transform.parent.parent;
            if(_barrel == null)
                _barrel = HierarchyUtils.GetFromAllChildren<BarrelThrowable>(parent).FirstOrDefault();
            if(_preyAnimator == null)
                _preyAnimator = HierarchyUtils.GetFromAllChildren<PreyAnimator>(parent).FirstOrDefault();
            if(_localMover == null)
                _localMover = HierarchyUtils.GetFromAllChildren<PackUnitLocalMover>(parent).FirstOrDefault();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
        public void Begin()
        {
            _preyAnimator.PlayByTrigger(_animationKeys.BarrelThrowAnimKey);
        }  

        // Animation Event
        public void OnBarrelThrown()
        {
            _barrel.Push();
            _localMover.RotateToPoint();
            OnEnded?.Invoke();
        }
        
        public void Stop()
        { }
        
        public event Action OnEnded;
        
        
    }
}