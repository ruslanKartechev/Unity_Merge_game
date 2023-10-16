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
        
        public bool Cast(Transform transform, out RaycastHit hit)
        {
            var castDistance = _settings.JumpSpeed * Time.deltaTime * 0.5f;
            Debug.DrawRay(_castFrom.position, transform.forward, Color.red, 3f);
            if (Physics.SphereCast(new Ray(_castFrom.position, transform.forward),
                    _settings.BiteCastRadius, out var mHit, castDistance, _mask))
            {
                hit = mHit;
                return true;
            }
            hit = default;
            return false;
        }
        

    }
}