using Game.Core;
using Game.Hunting;
using Game.Hunting.Prey.Interfaces;
using Game.UI;
using Game.UI.Hunting;
using UnityEngine;

namespace Game.Levels
{
    public class RewardCalculator : MonoBehaviour, IRewardCalculator
    {
        private IHuntUIPage _uiPage;
        private IPreyPack _preyPack;
        
        private float _totalRewardEarned = 0;
        private int _preyKilled;
        private int _totalPrey;
        
        public void Init(IPreyPack pack, IHuntUIPage ui)
        {
            _uiPage = ui;
            _preyPack = pack;
            _preyPack.OnPreyKilled += OnPreyKilled;
            _totalPrey = _preyPack.PreyCount;   
            _uiPage.SetKillCount(0, _totalPrey);
        }

        public void ResetReward()
        {
            _totalRewardEarned = 0f;
            UIC.Money.UpdateCount(false);
        }

        public void ApplyReward()
        {
            // reward has already been added after every kill
            // GC.PlayerData.Money += _totalRewardEarned;
            UIC.Money.UpdateCount(false);
        }

        public float TotalReward => _totalRewardEarned;

        private void OnPreyKilled(IPrey prey)
        {
            _preyKilled++;
            _uiPage.SetKillCount(_preyKilled, _totalPrey);
            var reward = prey.GetReward();
            _totalRewardEarned += reward;
            GC.PlayerData.Money += reward;
            var enemyPos = prey.CamTarget.GetPosition();
            _uiPage.FlyingMoney.FlySingle(enemyPos);
        }
    }
}