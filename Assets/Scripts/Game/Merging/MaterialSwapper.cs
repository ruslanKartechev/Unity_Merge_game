using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Merging
{
    public class MaterialSwapper : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _defaultMat;
        [SerializeField] private Material _switchedMat;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_defaultMat != null)
                return;
            GetMaterial();
        }

        [ContextMenu("Get Renderer Material")]
        public void GetMaterial()
        {
            if (_renderer == null)
                return;
            _defaultMat = _renderer.sharedMaterial;
            EditorUtility.SetDirty(this);   
        }
        #endif
        
        public void Switch()
        {
            _renderer.sharedMaterial = _switchedMat;
        }

        public void ReturnNormal()
        {
            _renderer.sharedMaterial = _defaultMat;
        }
    }
}