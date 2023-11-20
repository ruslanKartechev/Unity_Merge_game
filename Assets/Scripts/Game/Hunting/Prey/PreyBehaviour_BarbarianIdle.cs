using System;
using System.Linq;
using Common;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyBehaviour_BarbarianIdle : MonoBehaviour, IPreyBehaviour
    {
        [SerializeField] private PreyAnimator _preyAnimator;
        [SerializeField] private PreyAnimationKeys _animationKeys;
        [SerializeField] private PreyRandomWeaponPicker _randomWeaponPicker;
        
#if UNITY_EDITOR
        public bool _getRefs;
        private void OnValidate()
        {
            if (Application.isPlaying || !_getRefs)
                return;
            var parent = transform.parent.parent;
            if(_preyAnimator == null)
                _preyAnimator = HierarchyUtils.GetFromAllChildren<PreyAnimator>(parent).FirstOrDefault();
            if(_randomWeaponPicker == null)
                _randomWeaponPicker = HierarchyUtils.GetFromAllChildren<PreyRandomWeaponPicker>(parent).FirstOrDefault();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
        public void Begin()
        {
            _randomWeaponPicker.SetRandomWeapon();
            _preyAnimator.PlayByName(_animationKeys.IdleAnimKeys.Random(),UnityEngine.Random.Range(0f, 1f));
        }

        public void Stop()
        { }

        public event Action OnEnded;
    }
}