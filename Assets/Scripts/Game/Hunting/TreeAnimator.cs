using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class TreeAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Material[] _normalMats;
        [SerializeField] private Material[] _tranaparentMats;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private string _tag;        

        public void Stop()
        {
            _animator.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_tag))
                _renderer.sharedMaterials = _tranaparentMats;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_tag))
                _renderer.sharedMaterials = _normalMats;
        }
    }
}