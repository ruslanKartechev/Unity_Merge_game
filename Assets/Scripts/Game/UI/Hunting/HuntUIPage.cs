using System;
using Game.UI.Elements;
using Game.UI.Merging;
using UnityEngine;

namespace Game.Hunting.UI
{
    [DefaultExecutionOrder(12)]
    public class HuntUIPage : MonoBehaviour, IHuntUIPage
    {
        [SerializeField] private HuntingManager _huntingManager;
        [SerializeField] private MoneyUI _money;
        [SerializeField] private KillCountDisplayUI _killCountDisplay;
        [SerializeField] private DarkeningUI _darkening;
        [SerializeField] private WinPopup _winPopup;
        [SerializeField] private LoosePopup _failPopup;
        [SerializeField] private SuperEggUI _superEggUI;
        private bool _win;
        
        public ISuperEggUI SuperEggUI => _superEggUI;

        
        private void Start()
        {
            _money.UpdateCount(false);
            _huntingManager.Init(this);
            _darkening.HideNow();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_huntingManager == null)
                _huntingManager = FindObjectOfType<HuntingManager>();
        }
#endif
        
        public void UpdateMoney(float addedSum)
        {
            _money.UpdateCount(true);
        }

        public void SetKillCount(int killed, int target)
        {
            _killCountDisplay.SetKillCount(killed, target);   
        }

        public void Win(float award)
        {
            _win = true;
            _darkening.Show();
            _winPopup.SetAward(award);
            _winPopup.Show();
            _winPopup.SetOnClicked(Continue);
        }
        
        public void Fail()
        {
            _darkening.Show();
            _win = false;
            _failPopup.SetOnClicked(RestartFromMerge, ReplayThisLevel);
            _failPopup.Show();
        }
        
        private void Continue()
        {
            _winPopup.Hide(true, _huntingManager.Continue);
        }

        private void ReplayThisLevel()
        {
            _winPopup.Hide(true, _huntingManager.ReplayLevel);
        }
        
        private void RestartFromMerge()
        {
            _winPopup.Hide(true, _huntingManager.RestartFromMerge);
        }
        
        public void HidePopup(Action onEnd)
        {
            _darkening.Hide();
            if (_win)
                _winPopup.Hide(true, onEnd);
            else
                _failPopup.Hide(true, onEnd);
        }
        
    }
}