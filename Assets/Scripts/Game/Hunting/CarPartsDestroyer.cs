﻿using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class CarPartsDestroyer : MonoBehaviour
    {
        [SerializeField] private float _forceMin;
        [SerializeField] private float _forceMax;
        [SerializeField] private List<CarPart> _parts;
        [SerializeField] private List<GameObject> _hideTargets;
        [SerializeField] private ParticleSystem _destroyedParticles;
        [SerializeField] private List<CarPart> _windows;

        
        public void DestroyAllParts()
        {
            foreach (var go in _hideTargets)
                go.SetActive(false);
            foreach (var part in _parts)
                PushPart(part);
            _destroyedParticles.Play();
        }

        private void PushPart(CarPart part)
        {
            part.collider.enabled = true;
            part.collider.isTrigger = false;
            part.rb.isKinematic = false;
            var dir = RandomDirection();
            if (dir.y < 0)
                dir *= -1;
            part.rb.AddForce(dir * RandomForce(), ForceMode.VelocityChange);
        }
        
        public void DestroyWindow()
        {
            if (_windows.Count == 0)
                return;
            var part = _windows.Random();
            PushPart(part);
            _windows.Remove(part);
        }

        private float RandomForce() => UnityEngine.Random.Range(_forceMin, _forceMax);
        private Vector3 RandomDirection() => UnityEngine.Random.onUnitSphere;
        
        [System.Serializable]
        public class CarPart
        {
            public Rigidbody rb;
            public Collider collider;
        }
    }
}