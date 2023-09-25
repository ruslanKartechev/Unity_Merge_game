using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Hunting
{
    public class CollidersSwitch : MonoBehaviour
    {
        [SerializeField] private List<Collider> _colliders;
#if UNITY_EDITOR
        [Header("Used in editor to get all colliders")]
        [SerializeField] private List<GameObject> _gameObjects;

        [ContextMenu("Get colliders")]
        public void Get()
        {
            _colliders = new List<Collider>();
            foreach (var go in _gameObjects)
                _colliders.AddRange(go.GetComponents<Collider>());
            EditorUtility.SetDirty(this);
        }
#endif

        public void On()
        {
            foreach (var collider in _colliders)
                collider.enabled = true;
        }
        
        public void Off()
        {
            foreach (var collider in _colliders)
                collider.enabled = false;
        }
    }
}