using Game.Saving;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class PregameCheat : MonoBehaviour
    {
        [SerializeField] private OptionSection _tutorsSection;
        [SerializeField] private OptionSection _levelSection;
        [SerializeField] private OptionSection _moneySection;
        [SerializeField] private Button _clearSavesButton;
        [Space(10)] 
        [SerializeField] private GameObject _block;
        [SerializeField] private Button _playButton;
        [SerializeField] private SavedDataInitializer _dataInitializer;
        
        public void Show(UnityAction onClose)
        {
            _block.SetActive(true);
            _playButton.onClick.AddListener(onClose);
            
            var playerData = GC.PlayerData;
            _tutorsSection.Init(DisableTutors, EnableTutors);
            _tutorsSection.Output(!playerData.TutorPlayed_Attack);
            
            _levelSection.Init(PrevLevel, NextLevel);
            _levelSection.Output(playerData.LevelTotal + 1);
            
            _moneySection.Init(LessMoney, MoreMoney);
            _moneySection.OutputMoney(playerData.Money);
            
            _clearSavesButton.onClick.AddListener(() =>
            {
                GC.DataSaver.Clear();
            });
        }

        public void Hide()
        {
            _block.SetActive(false);
        }

        private void EnableTutors()
        {
            GC.PlayerData.TutorPlayed_Attack = false;
            GC.PlayerData.TutorPlayed_Merge = false;
            _tutorsSection.Output(true);   
        }

        private void DisableTutors()
        {
            GC.PlayerData.TutorPlayed_Attack = true;
            GC.PlayerData.TutorPlayed_Merge = true;
            _tutorsSection.Output(false);
        }

        private void NextLevel()
        {
            var levelsTotal = GC.PlayerData.LevelTotal;
            var levelsIndex = GC.PlayerData.LevelIndex;
            if (levelsIndex <= levelsTotal)
            {
                levelsIndex++;
            }
            levelsTotal++;
            GC.PlayerData.LevelTotal = levelsTotal;
            GC.PlayerData.LevelIndex = levelsIndex;
            _levelSection.Output(levelsTotal + 1);
        }

        private void PrevLevel()
        {
            var levelsTotal = GC.PlayerData.LevelTotal;
            var levelsIndex = GC.PlayerData.LevelIndex;
            levelsIndex--;
            levelsTotal--;
            if(levelsIndex < 0)
                levelsIndex = 0;
            if (levelsTotal < 0)
                levelsTotal = 0;
            GC.PlayerData.LevelTotal = levelsTotal;
            GC.PlayerData.LevelIndex = levelsIndex;
            _levelSection.Output(levelsTotal + 1);
        }
        
        private void MoreMoney()
        {
            GC.PlayerData.Money += 100;
            _moneySection.OutputMoney(GC.PlayerData.Money);
        }

        private void LessMoney()
        {
            GC.PlayerData.Money -= 100;
            if (GC.PlayerData.Money < 0)
                GC.PlayerData.Money = 0;
            _moneySection.OutputMoney(GC.PlayerData.Money);   
        }
        
    }
}