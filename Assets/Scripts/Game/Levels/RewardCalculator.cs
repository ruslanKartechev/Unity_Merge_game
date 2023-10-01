using Game.Hunting;
using Game.Hunting.UI;
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
            _uiPage.UpdateMoney();
        }

        public void ApplyReward()
        {
            GC.PlayerData.Money += _totalRewardEarned;
            _uiPage.UpdateMoney();
        }

        public float TotalReward => _totalRewardEarned;

        private void OnPreyKilled(IPrey prey)
        {
            _preyKilled++;
            _uiPage.SetKillCount(_preyKilled, _totalPrey);
            var reward = prey.GetReward();
            _totalRewardEarned += reward;
            // GC.PlayerData.Money += reward;
            _uiPage.UpdateMoney(_totalRewardEarned);
        }
        
        
    }
}