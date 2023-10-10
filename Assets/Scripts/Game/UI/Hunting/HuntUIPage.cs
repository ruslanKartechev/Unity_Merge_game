using System;
using System.Collections;
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
        [SerializeField] private LevelDisplay _levelDisplay;
        [Space(10)]
        [SerializeField] private DarkeningUI _darkening;
        [SerializeField] private WinPopup _winPopup;
        [SerializeField] private LoosePopup _failPopup;
        [SerializeField] private SuperEggUI _superEggUI;
        [SerializeField] private PowerDisplay _powerDisplay;
        private bool _darkened;
        
        public ISuperEggUI SuperEggUI => _superEggUI;
       
        
        public void Darken()
        {
            if (_darkened)
                return;
            _darkened = true;
            _darkening.Show();
        }

        private void Start()
        {
            _money.UpdateCount(false);
            _huntingManager.Init(this);
            _levelDisplay.SetCurrent();
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
            Darken();
            _winPopup.SetAward(award);
            _winPopup.Show();
            _winPopup.SetOnClicked(Continue);
        }
        
        public void Fail()
        {
            Darken();
            _failPopup.SetOnClicked(RestartFromMerge, ReplayThisLevel);
            _failPopup.Show();
        }
        
        private void Continue()
        {
            _huntingManager.Continue();
        }

        private void ReplayThisLevel()
        {
            _huntingManager.ReplayLevel();
        }
        
        private void RestartFromMerge()
        {
            _huntingManager.RestartFromMerge();
        }
        
        public void ShowPower(float ourPower, float enemyPower, float duration)
        {
            _powerDisplay.SetPower(ourPower, enemyPower);
            _powerDisplay.Show();
            StartCoroutine(DelayedAction(_powerDisplay.Hide, duration));
        }

        private IEnumerator DelayedAction(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}