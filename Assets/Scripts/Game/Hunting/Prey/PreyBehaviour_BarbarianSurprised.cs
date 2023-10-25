using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyBehaviour_BarbarianSurprised : MonoBehaviour, IPreyBehaviour
    {
        public event Action OnEnded;

        [SerializeField] private float _rotationDelay;
        [SerializeField] private PackUnitLocalMover _localMover;
        [SerializeField] private PreyAnimationKeys _animationKeys;
        [SerializeField] private PreyAnimator _preyAnimator;
        
                
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
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
            _preyAnimator.PlayByTrigger(_animationKeys.ScaredAnimKeys[0]);
            StartCoroutine(DelayedRotate());
        }

        public void Stop()
        {
            _localMover.StopMoving();
            _localMover.StopRotating();
        }

        private IEnumerator DelayedRotate()
        {
            yield return new WaitForSeconds(_rotationDelay);
            _localMover.RotateToPoint();
        }
    }
}