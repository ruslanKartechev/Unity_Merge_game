using Game.Saving;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class PregameCheat : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private OptionSection _levelSection;
        [SerializeField] private OptionSection _moneySection;
        [SerializeField] private Button _clearSavesButton;
        [Space(10)] 
        [SerializeField] private GameObject _block;
        [SerializeField] private Button _playButton;
        [SerializeField] private SavedDataInitializer _dataInitializer;
        private bool _tutorVal;

        
        public void Show(UnityAction onClose)
        {
            _block.SetActive(true);
            _playButton.onClick.AddListener(onClose);
            
            var playerData = GC.PlayerData;
            _tutorVal = playerData.TutorPlayed_Purchased || playerData.TutorPlayed_Attack ||
                        playerData.TutorPlayed_Merge;
            _toggle.onValueChanged.AddListener(OnValueChange);
            _toggle.isOn = _tutorVal;
            
            _levelSection.Init(PrevLevel, NextLevel);
            _levelSection.Output(playerData.LevelIndex + 1);
            
            _moneySection.Init(LessMoney, MoreMoney);
            _moneySection.OutputMoney(playerData.Money);
            
            _clearSavesButton.onClick.AddListener(() =>
            {
                GC.DataSaver.Clear();
            });
        }

        private void OnValueChange(bool value)
        {
            _tutorVal = value;
            GC.PlayerData.TutorPlayed_Attack 
                = GC.PlayerData.TutorPlayed_Attack 
                = GC.PlayerData.TutorPlayed_Attack 
                = _tutorVal;
        }

        public void Hide()
        {
            _block.SetActive(false);
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
            _levelSection.Output(levelsIndex + 1);
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
            _levelSection.Output(levelsIndex + 1);
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