using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class SmallFishTank : MonoBehaviour
    {
        [SerializeField] private float _forceMin;
        [SerializeField] private float _forceMax;
        [SerializeField] private List<SmallFishModel> _smallFishModels;
        [SerializeField] private CircleRotator _circleRotator;

        public void Idle()
        {
            _circleRotator.Begin();
        }
        
        public void PushRandomDir()
        {
            foreach (var fish in _smallFishModels)
            {
                var fm = UnityEngine.Random.Range(_forceMin, _forceMax);
                var force = UnityEngine.Random.onUnitSphere * fm;
                if (force.y < 0)
                    force.y *= -1f;
                fish.Push(force);
            }
            StartCoroutine(DelayedScaleDown());
        }
        
        public void AlignToAttack()
        {
            _circleRotator.RotateToStrait(.3f);
        }

        private IEnumerator DelayedScaleDown()
        {
            var delay = 2f;
            var scaleTime = 0.33f;
            yield return new WaitForSeconds(delay);
            var elapsed = 0f;
            foreach (var fish in _smallFishModels)
                fish.NoPhys();
            
            while (elapsed <= scaleTime)
            {
                var t = elapsed / scaleTime;
                foreach (var fish in _smallFishModels)
                    fish.ScaleTo0(t);
                elapsed += Time.deltaTime;
                yield return null;
            }
            foreach (var fish in _smallFishModels)
                fish.gameObject.SetActive(false);
        }
    }
}