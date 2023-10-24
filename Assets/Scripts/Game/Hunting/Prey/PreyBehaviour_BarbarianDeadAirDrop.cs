using System;
using System.Linq;
using Common.Ragdoll;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyBehaviour_BarbarianDeadAirDrop : MonoBehaviour, IPreyBehaviour
    {
        public event Action OnEnded;
        
        [SerializeField] private DeadColorPainter _deadColor;
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private RagdollBodyPusher _ragdollBodyPusher;
        
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
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
        public void Begin()
        {
            transform.SetParent(null);
            _deadColor.PaintDead();
            _ragdoll.Activate();
        }

        public void Stop()
        {
        }
    }
}