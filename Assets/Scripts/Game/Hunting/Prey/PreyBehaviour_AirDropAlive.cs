using System;
using System.Collections;
using System.Linq;
using Common.Ragdoll;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyBehaviour_AirDropAlive : MonoBehaviour, IPreyBehaviour
    {
        public event Action OnEnded;
        
        [SerializeField] private DeadColorPainter _deadColor;
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private RagdollBodyPusher _ragdollBodyPusher;
        [SerializeField] private PreyAnimator _animator;
        [SerializeField] private PreyAnimationKeys _animationKeys;
        [SerializeField] private OnTerrainPositionAdjuster _positionAdjuster;
        [SerializeField] private PreyHealth _health;
        private Transform _startParent;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            var parent = transform.parent.parent;
            if(_ragdoll == null)
                _ragdoll = HierarchyUtils.GetFromAllChildren<IRagdoll>(parent).FirstOrDefault();
            if(_ragdollBodyPusher == null)
                _ragdollBodyPusher = HierarchyUtils.GetFromAllChildren<RagdollBodyPusher>(parent).FirstOrDefault();
            if(_deadColor == null)
                _deadColor = HierarchyUtils.GetFromAllChildren<DeadColorPainter>(parent).FirstOrDefault();
            if(_animator == null)
                _animator = HierarchyUtils.GetFromAllChildren<PreyAnimator>(parent).FirstOrDefault();
            if(_health == null)
                _health = HierarchyUtils.GetFromAllChildren<PreyHealth>(parent).FirstOrDefault();
            if(_positionAdjuster == null)
                _positionAdjuster = HierarchyUtils.GetFromAllChildren<OnTerrainPositionAdjuster>(parent).FirstOrDefault();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        private void Start()
        {
            _startParent = transform.parent.parent.parent;
        }

        public void Begin()
        {
            _animator.PlayByTrigger(_animationKeys.FallFromAir);
            _health.Show();
            transform.parent.parent.SetParent(_startParent);
            StartCoroutine(MovingDown());
        }

        public void Stop()
        {
            StopAllCoroutines();
        }

        private IEnumerator MovingDown()
        {
            var time = .3f;
            var elapsed = 0f;
            var movable = transform.parent.parent;
            var startY = movable.position.y;
            while (elapsed <= time)
            {
                var targetY = _positionAdjuster.GetAdjustedPosition(movable.position).y;
                var pos = movable.position;
                pos.y = Mathf.Lerp(startY, targetY, elapsed / time);
                movable.position = pos;
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            _positionAdjuster.enabled = true;
            _animator.ForceInjured();
        }
    }
}