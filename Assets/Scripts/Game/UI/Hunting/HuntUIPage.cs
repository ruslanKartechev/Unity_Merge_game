using System;
using System.Collections;
using System.Runtime.InteropServices;
using Game.UI.Elements;
using Game.UI.Merging;
using Common;
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
        [SerializeField] private FlyingMoney _flyingMoney;
        [Space(10)]
        [SerializeField] private SuperEggUI _superEggUI;
        [SerializeField] private PowerDisplay _powerDisplay;
        [SerializeField] private ProperButton _inputButton;
        private bool _darkened;
        
        public ISuperEggUI SuperEggUI => _superEggUI;
        public IFlyingMoney FlyingMoney => _flyingMoney;

        public ProperButton InputButton => _inputButton;
        
        private void Start()
        {
            _money.UpdateCount(false);
            _huntingManager.Init(this);
            _levelDisplay.SetCurrent();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_huntingManager == null)
                _huntingManager = FindObjectOfType<HuntingManager>();
        }
#endif
        

        public void SetKillCount(int killed, int target)
        {
            _killCountDisplay.SetKillCount(killed, target);   
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