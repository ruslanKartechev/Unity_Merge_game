using System.Collections;
using System.Linq;
using Common.Utils;
using UnityEditor;
using UnityEngine;

namespace Common
{
    public class RendererColorer : MonoBehaviour
    {
        private const string ColorKey = "_Color";
        [SerializeField] private SkinnedMeshRenderer _renderer;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_renderer == null)
            {
                _renderer = HierarchyUtils.GetFromAllChildren<SkinnedMeshRenderer>(transform).First();
                EditorUtility.SetDirty(this);
            }
        }
        #endif

        public void SetColor(Color color)
        {
            if (_renderer.sharedMaterials.Length == 1)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                _renderer.GetPropertyBlock(block);
                block.SetColor(ColorKey, color);
                _renderer.SetPropertyBlock(block);
                Debug.Log($"Setting color {ColorKey} to: {color}");
                return;
            }
            var count = _renderer.sharedMaterials.Length;
            for (var i = 0; i < count; i++)
                SetColorToIndex(color, i);
        }

        public void FadeToColor(Color color, float time)
        {
            var count = _renderer.sharedMaterials.Length;
            var initialColors = new Color[count];
            for (var i = 0; i < count; i++)
                initialColors[i] = _renderer.sharedMaterials[i].color;
            StartCoroutine(Fading(initialColors, color, time, count));
        }

        private IEnumerator Fading(Color[] from, Color to, float time, int count)
        {
            var elapsed = 0f;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                for (var i = 0; i < count; i++)
                {
                    var c = Color.Lerp(from[i], to, t);
                    SetColorToIndex(c,i);
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        
        private void SetColorToIndex(Color color, int index)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(block, index);
            block.SetColor(ColorKey, color);   
            _renderer.SetPropertyBlock(block);
        }
    }
}