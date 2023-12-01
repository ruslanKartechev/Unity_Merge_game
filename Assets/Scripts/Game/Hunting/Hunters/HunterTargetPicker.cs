using Game.Hunting.HuntCamera;
using Game.Hunting.Hunters.Interfaces;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    public class HunterTargetPicker
    {
        private IPreyPack _preyPack;

        public HunterTargetPicker(IPreyPack pack)
        {
            _preyPack = pack;
        }
        
        public bool PickHunterCamTarget(IHunter hunter, out ICamFollowTarget camTarget)
        {
            camTarget = _preyPack.CamTarget;
            return true;
            // var prey = GetBestPrey(hunter);
            // if (prey != null)
            // {
            //     camTarget = prey.CamTarget;
            //     return true;
            // }
            // return false;
        }

        public IPrey GetBestPrey(IHunter hunter)
        {
            var preyAll = _preyPack.GetPrey();
            var bestDamageDiff = float.MaxValue;
            var damage = hunter.Settings.Damage;
            IPrey bestPrey = null;
            foreach (var prey in preyAll)
            {
                if(prey.IsAvailableTarget == false)
                    continue;
                var diff = Mathf.Abs(prey.PreySettings.Health - damage);
                if (diff < bestDamageDiff)
                {
                    bestDamageDiff = diff;
                    bestPrey = prey;
                }
            }
            return bestPrey;
        }
    }
}