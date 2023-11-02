using System;
using System.Linq;
using Common.Ragdoll;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyBehaviour_BarbarianDead : MonoBehaviour, IPreyBehaviour
    {
        public event Action OnEnded;
        [SerializeField] private Transform _movableBody;
        [SerializeField] private DeadColorPainter _deadColor;
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private RagdollBodyPusher _ragdollBodyPusher;
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PreyRandomWeaponPicker _weaponPicker;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            var parent = transform.parent.parent;
            if (_movableBody == null)
                _movableBody = transform.parent.parent;
            if(_ragdoll == null)
                _ragdoll = HierarchyUtils.GetFromAllChildren<IRagdoll>(parent).FirstOrDefault();
            if(_ragdollBodyPusher == null)
                _ragdollBodyPusher = HierarchyUtils.GetFromAllChildren<RagdollBodyPusher>(parent).FirstOrDefault();
            if(_deadColor == null)
                _deadColor = HierarchyUtils.GetFromAllChildren<DeadColorPainter>(parent).FirstOrDefault();
            if(_preyAnimator == null)
                _preyAnimator = HierarchyUtils.GetFromAllChildren<PreyAnimator>(parent).FirstOrDefault();
            if(_weaponPicker == null)
                _weaponPicker = HierarchyUtils.GetFromAllChildren<PreyRandomWeaponPicker>(parent).FirstOrDefault();
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
        public void Begin()
        {
            _movableBody.SetParent(null);
            _preyAnimator.Disable();
            _deadColor.PaintDead();
            _ragdoll.Activate();
            _ragdollBodyPusher.Push(_movableBody.forward);
            _weaponPicker.DropWeapon();
        }

        public void Stop()
        {
        }
    }
}