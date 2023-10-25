using System;
using System.Linq;
using Common.Utils;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyBehaviour_TreeCutIdle : MonoBehaviour, IPreyBehaviour
    {
        public event Action OnEnded;

        [SerializeField] private PreyAnimationKeys _animationKeys;
        [SerializeField] private PreyAnimator _animator;
        [SerializeField] private TreeAnimator _treeAnimator;
        
        
#if UNITY_EDITOR
        [Space(20)] 
        [SerializeField] private bool _getTree;
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            var parent = transform.parent.parent;
            if(_animator == null)
                _animator = HierarchyUtils.GetFromAllChildren<PreyAnimator>(parent).FirstOrDefault();
            if(_getTree && _treeAnimator == null)
                _treeAnimator = FindObjectOfType<TreeAnimator>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        
        public void Begin()
        {
            _animator.PlayByName(_animationKeys.TreeCutAnimKey);   
        }

        public void Stop()
        {
            _treeAnimator.StopAnimator();
            _treeAnimator.transform.parent = null;   
        }

    }
}