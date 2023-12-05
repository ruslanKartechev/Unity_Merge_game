using System;
using System.Collections;
using Common.Ragdoll;
using UnityEngine;

namespace Creatives.Kong
{
    public class ThrowableHuman : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _screamAnim;
        [SerializeField] private string _grabbedAnim;
        [SerializeField] private Ragdoll _ragdoll;
        [SerializeField] private Transform _reparentTarget;
        [SerializeField] private Transform _root;
        [SerializeField] private float _rotTime = .2f;
        [SerializeField] private Vector3 _localPos;


        private void Start()
        {
            _animator.Play(_screamAnim);
        }

        public void Grab(Transform parent)
        {
            _animator.Play(_grabbedAnim);
            _root.parent = parent;
            StartCoroutine(Rotating());
        }

        public void Throw(Vector3 force)
        {
            Debug.Log("Human Thrown");
            _animator.enabled = false;
            _reparentTarget.parent = null;
            _ragdoll.ActivateAndPush(force);
        }
        
        private IEnumerator Rotating()
        {
            var time = _rotTime;
            var elapsed = 0f;
            var tr = _root;
            var rot1 = tr.localRotation;
            var rot2 = Quaternion.identity;
            var pos1 = tr.localPosition;
            var pos2 = _localPos;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                tr.localRotation = Quaternion.Lerp(rot1, rot2, t);
                tr.localPosition = Vector3.Lerp(pos1, pos2, t);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}