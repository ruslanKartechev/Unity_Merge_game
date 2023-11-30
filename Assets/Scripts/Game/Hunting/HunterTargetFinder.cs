using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public class HunterTargetFinder
    {
        private Transform _castFrom;
        private IHunterSettings _settings;
        private LayerMask _mask;
        
        
        public HunterTargetFinder(Transform castFrom, IHunterSettings settings, LayerMask mask)
        {
            _castFrom = castFrom;
            _settings = settings;
            _mask = mask;
        }
        
        public bool Cast(Transform transform, out Collider hit)
        {
            var castDistance = _settings.JumpSpeed * Time.deltaTime;
            // Debug.DrawRay(_castFrom.position, transform.forward, Color.red, 3f);
            var overlaps = Physics.OverlapSphere(_castFrom.position, _settings.BiteCastRadius, _mask);
            if(overlaps.Length > 0)
            {
                hit = overlaps[0];
                return true;
            }
            hit = default;
            return false;
        }
        

    }
}