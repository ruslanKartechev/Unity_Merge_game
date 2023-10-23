using System.Collections.Generic;
using Game.Hunting.Hunters.Interfaces;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public class TargetSeeker_Fish
    {
        private Transform _castFrom;
        private IHunterSettings _settings;
        private LayerMask _mask;
        private SmallFishTank _fishTank;

        private const float FishFlyTime = .5f;

        public TargetSeeker_Fish(Transform castFrom, IHunterSettings settings, LayerMask mask, SmallFishTank fishTank)
        {
            _castFrom = castFrom;
            _settings = settings;
            _mask = mask;
            _fishTank = fishTank;
        }
        
        public void Attack()
        {
            var radius = _settings.Radius;
            var overlaps = Physics.OverlapSphere(_castFrom.position, radius, _mask);
            var count = 0;
            var targets = new List<IFishTarget>();
            Debug.Log($"Fish attack. Radius: {radius}, Damage: {_settings.Damage}");
            foreach (var collider in overlaps)
            {
                var target = collider.gameObject.GetComponent<IFishTarget>();
                if (target == null
                    || target.IsAlive() == false)
                    continue;
                target.Damage(new DamageArgs(_settings.Damage, target.GetShootAtPosition()));
                targets.Add(target);
                count++;
            }
            
            if (targets.Count == 0)
            {
                _fishTank.PushRandomDir();
                return;
            }
            _fishTank.StopAnimations();
            var fish = _fishTank.Fish;
            var fishIndex = 0;
            var fishCount = fish.Count;
            while (fishIndex < fishCount)
            {
                for (var i = 0; i < targets.Count; i++)
                {
                    if (fishIndex >= fishCount)
                        break;
                    fish[fishIndex].FlyTo(targets[i].GetShootAtPosition(), FishFlyTime);
                    fishIndex++;
                }              
            }
            Debug.Log($"Damaged count: {count}, fish count: {fishCount}");
                
        }
        
    }
}