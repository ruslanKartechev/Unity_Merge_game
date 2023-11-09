using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public class CameraTargetPicker
    {
        private IPreyPack _preyPack;

        public CameraTargetPicker(IPreyPack pack)
        {
            _preyPack = pack;
        }
        
        public ICamFollowTarget PickHunterCamTarget(IHunter hunter)
        {
            var camTarget = _preyPack.CamTarget;
            var prey = GetBestPrey(hunter);
            if (prey != null)
                camTarget = prey.CamTarget;
            return camTarget;
        }

        public IPrey GetBestPrey(IHunter hunter)
        {
            var preyAll = _preyPack.GetPrey();
            var bestDamageDiff = float.MaxValue;
            var damage = hunter.Settings.Damage;
            IPrey bestPrey = null;
            foreach (var prey in preyAll)
            {
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