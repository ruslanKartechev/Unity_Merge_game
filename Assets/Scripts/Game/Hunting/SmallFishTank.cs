﻿using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class SmallFishTank : MonoBehaviour
    {
        [SerializeField] private float _forceMin;
        [SerializeField] private float _forceMax;
        [SerializeField] private List<SmallFishModel> _smallFishModels;

        public void PushRandomDir()
        {
            foreach (var fish in _smallFishModels)
            {
                var fm = UnityEngine.Random.Range(_forceMin, _forceMax);
                var force = UnityEngine.Random.onUnitSphere * fm;
                // Debug.Log($"Force: {force}");
                if (force.y < 0)
                    force.y *= -1f;
                fish.Push(force);
            }
        }
    }
}