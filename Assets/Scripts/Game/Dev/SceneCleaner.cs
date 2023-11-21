using UnityEngine;

namespace Game.Dev
{
    public class SceneCleaner : MonoBehaviour
    {
        
        [ContextMenu("Clear ALL")]
        public void Clear()
        {
            ClearMeshColliders();        
            ClearLights();
            ClearAnimations();
            ClearBoxes();
        }

        [ContextMenu("Clear AudioSource")]
        public void ClearAudioSources()
        {
            var items = FindObjectsOfType<AudioSource>();
            foreach (var item in items)
                DestroyImmediate(item);   
        }

        [ContextMenu("Clear MeshColliders")]
        public void ClearMeshColliders()
        {
            var items = FindObjectsOfType<MeshCollider>();
            foreach (var item in items)
                DestroyImmediate(item);
        }

        [ContextMenu("Clear Lights")]
        public void ClearLights()
        {
            var items = FindObjectsOfType<MeshCollider>();
            foreach (var item in items)
                DestroyImmediate(item);
        }

        [ContextMenu("Clear Animations")]
        public void ClearAnimations()
        {
            var items = FindObjectsOfType<Animation>();
            foreach (var item in items)
                DestroyImmediate(item);   
        }
        
        
        public void ClearBoxes()
        {
            var items = FindObjectsOfType<BoxCollider>();
            foreach (var item in items)
                DestroyImmediate(item);   
        }

    }
}