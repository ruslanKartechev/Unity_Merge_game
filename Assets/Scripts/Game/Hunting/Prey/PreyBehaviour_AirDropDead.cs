using System;
using System.Linq;
using Common.Ragdoll;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyBehaviour_AirDropDead : MonoBehaviour, IPreyBehaviour
    {
        public event Action OnEnded;
        
        [SerializeField] private DeadColorPainter _deadColor;
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private RagdollBodyPusher _ragdollBodyPusher;
        [SerializeField] private PreyAnimator _animator;
        
#if UNITY_EDITOR
        public bool _getRefs;
        private void OnValidate()
        {
            if (Application.isPlaying ||!_getRefs)
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
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
        public void Begin()
        {
            _animator.Disable();
            transform.parent.parent.SetParent(null);
            _deadColor.PaintDead();
            _ragdoll.Activate();
        }

        public void Stop()
        {
        }
    }
}