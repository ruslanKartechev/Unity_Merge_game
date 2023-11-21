﻿using Game.Hunting.Hunters.Interfaces;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public class TargetSeeker_Predator
    {
        private Transform _castFrom;
        private IHunterSettings _settings;
        private LayerMask _mask;
        
        
        public TargetSeeker_Predator(Transform castFrom, IHunterSettings settings, LayerMask mask)
        {
            _castFrom = castFrom;
            _settings = settings;
            _mask = mask;
        }
        
        public bool GetHit(Transform transform, out RaycastHit hit)
        {
            var castDistance = _settings.Radius / 2f;
            // Debug.DrawRay(_castFrom.position, transform.forward, Color.red, 3f);
            if (Physics.SphereCast(new Ray(_castFrom.position, transform.forward),
                    _settings.Radius, 
                    out var mHit, 
                    castDistance, 
                    _mask))
            {
                hit = mHit;
                return true;
            }
            hit = default;
            return false;
        }
    }
}