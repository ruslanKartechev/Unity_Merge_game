using System;
using System.Linq;
using Common;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyBehaviour_BarbarianRun : MonoBehaviour, IPreyBehaviour
    {
        public event Action OnEnded;
        
        [SerializeField] private PreyAnimationKeys _animationKeys;
        [SerializeField] private PackUnitLocalMover _localMover;
        [SerializeField] private PreyAnimator _preyAnimator;

#if UNITY_EDITOR
        public bool _getRefs;

        private void OnValidate()
        {
            if (Application.isPlaying || !_getRefs)
                return;
            var parent = transform.parent.parent;
            if(_preyAnimator == null)
                _preyAnimator = HierarchyUtils.GetFromAllChildren<PreyAnimator>(parent).FirstOrDefault();
            if(_localMover == null)
                _localMover = HierarchyUtils.GetFromAllChildren<PackUnitLocalMover>(parent).FirstOrDefault();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
        
        public void Begin()
        {
            _preyAnimator.SetOverride(_animationKeys.RunOverrideControllers.Random());
            _preyAnimator.PlayByTrigger(_animationKeys.RunTriggerKey);
            _localMover.MoveToLocalPoint();
        }

        public void Stop()
        {
            _localMover.StopMoving();
            _localMover.StopRotating();
        }
        
    }
}