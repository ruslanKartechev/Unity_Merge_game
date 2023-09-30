using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Hunting
{
    public class CarPartsDestroyer : MonoBehaviour
    {
        [SerializeField] private float _forceMin;
        [SerializeField] private float _forceMax;
        [SerializeField] private List<CarPart> _parts;
        [SerializeField] private List<CarPart> _activateOnlyParts;
        [SerializeField] private List<GameObject> _hideTargets;
        [SerializeField] private ParticleSystem _destroyedParticles;
        [SerializeField] private List<CarPart> _windows;
        private HashSet<CarPart> _windowsActive;

        private void Awake()
        {
            _windowsActive = new HashSet<CarPart>(_windows.Count);
            foreach (var part in _windows)
                _windowsActive.Add(part);
        }

        public void DestroyAllParts()
        {
            foreach (var go in _hideTargets)
                go.SetActive(false);
            foreach (var part in _parts)
                PushPart(part);
            foreach (var part in _windowsActive)
                PushPart(part);
            foreach (var part in _activateOnlyParts)
            {
                part.collider.enabled = true;
                part.rb.isKinematic = true;
            }
            _destroyedParticles.Play();
        }

        private void PushPart(CarPart part, bool local = false)
        {
            part.collider.enabled = true;
            part.collider.isTrigger = false;
            part.rb.isKinematic = false;
            var dir = RandomDirection();
            if (dir.y < 0)
                dir *= -1;
            var force = part.pushDirection * RandomForce();
            if (local)
                force = part.rb.transform.TransformVector(force);
            part.rb.AddForce(force, ForceMode.VelocityChange);
        }
        
        public void DestroyWindow()
        {
            if (_windowsActive.Count == 0)
                return;
            var part = _windowsActive.FirstOrDefault();
            PushPart(part, true);
            _windowsActive.Remove(part);
        }

        private float RandomForce() => UnityEngine.Random.Range(_forceMin, _forceMax);
        private Vector3 RandomDirection() => UnityEngine.Random.onUnitSphere;
        
        
        #if UNITY_EDITOR

        [ContextMenu("GenerateRandomDirection")]
        public void GenerateRandomDirection()
        {
            foreach (var part in _parts)
                part.pushDirection = UnityEngine.Random.onUnitSphere;
            EditorUtility.SetDirty(this);
        }
        #endif
        
        
        [System.Serializable]
        public class CarPart
        {
            public Rigidbody rb;
            public Collider collider;
            public Vector3 pushDirection;
        }
        

    }
}