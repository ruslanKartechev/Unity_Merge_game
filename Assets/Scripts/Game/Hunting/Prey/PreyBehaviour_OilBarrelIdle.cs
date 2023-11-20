using System;
using System.Linq;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyBehaviour_OilBarrelIdle: MonoBehaviour, IPreyBehaviour
    {
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PreyAnimationKeys _animationKeys;

                
#if UNITY_EDITOR
        public bool _getRefs;
        private void OnValidate()
        {
            if (Application.isPlaying || !_getRefs)
                return;
            var parent = transform.parent.parent;
            if(_preyAnimator == null)
                _preyAnimator = HierarchyUtils.GetFromAllChildren<PreyAnimator>(parent).FirstOrDefault();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
        public void Begin()
        {
            _preyAnimator.PlayByName(_animationKeys.BarrelIdleAnimKey);
        }
        
        public void Stop()
        {}
        
        public event Action OnEnded;
    }
}