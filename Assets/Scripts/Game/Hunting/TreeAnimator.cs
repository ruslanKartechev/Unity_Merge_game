using System.Collections;
using UnityEngine;

namespace Game.Hunting
{
    public class TreeAnimator : MonoBehaviour
    {
        [SerializeField] private float _materialBackDelay = 2f;
        [SerializeField] private Animator _animator;
        [SerializeField] private Material[] _normalMats;
        [SerializeField] private Material[] _tranaparentMats;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private string _tag;        
        private Coroutine _delayedAction;

        
        public void StopAnimator()
        {
            if (_animator == null)
                return;
            _animator.StopPlayback();
            _animator.enabled = false;
            Destroy(_animator);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_tag))
            {
                _renderer.sharedMaterials = _tranaparentMats;
                StopDelayed();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_tag))
                ReturnMats();
        }


        private void ReturnMats()
        {
            StopDelayed();
            _delayedAction = StartCoroutine(DelayedMaterialReturn());
        }
        private void StopDelayed()
        {
            if(_delayedAction != null)
                StopCoroutine(_delayedAction);
        }
        
        private IEnumerator DelayedMaterialReturn()
        {
            yield return new WaitForSeconds(_materialBackDelay);
            _renderer.sharedMaterials = _normalMats;
        }
    }
}