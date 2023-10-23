using Game.Hunting.Hunters.Interfaces;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public class TargetSeeker_Air
    {
        private Transform _castFrom;
        private IHunterSettings _settings;
        private LayerMask _mask;
        private bool _hasTarget;
        
        public TargetSeeker_Air(Transform castFrom, IHunterSettings settings, LayerMask mask)
        {
            _castFrom = castFrom;
            _settings = settings;
            _mask = mask;
        }
      
        public bool GetTargetForward(Transform origin, out IAirTarget target)
        {
            var castDistance = _settings.JumpSpeed * Time.deltaTime;
            // Debug.DrawRay(_castFrom.position, Vector3.Down, Color.red, 1.5f);
            if (Physics.SphereCast(new Ray(_castFrom.position, origin.forward),
                    _settings.Radius, out var hit, castDistance, _mask))
            {
                target = hit.collider.GetComponent<IAirTarget>();
                return target != null;
            }
            target = null;
            return false;
        }
        
        public bool GetTargetDown(out IAirTarget target)
        {
            var castDistance = _settings.JumpSpeed * Time.deltaTime;
            // Debug.DrawRay(_castFrom.position, Vector3.Down, Color.red, 1.5f);
            if (Physics.SphereCast(new Ray(_castFrom.position, Vector3.down),
                    _settings.Radius, out var hit, castDistance, _mask))
            {
                target = hit.collider.GetComponent<IAirTarget>();
                return target != null;
            }
            target = null;
            return false;
        }

    }
}