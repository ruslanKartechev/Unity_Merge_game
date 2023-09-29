using System;
using Game.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hunting.UI
{
    [DefaultExecutionOrder(12)]
    public class HuntUIPage : MonoBehaviour, IHuntUIPage
    {
        [SerializeField] private HuntingManager _huntingManager;
        [SerializeField] private MoneyUI _money;
        [SerializeField] private KillCountDisplayUI _killCountDisplay;
        [SerializeField] private WinPopup _winPopup;
        [SerializeField] private LoosePopup _failPopup;
        [SerializeField] private Image _darkening;
        
        private bool _win;
        
        private void Start()
        {
            _money.UpdateCount(false);
            _huntingManager.Init(this);
            _darkening.enabled = false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_huntingManager == null)
                _huntingManager = FindObjectOfType<HuntingManager>();
        }
#endif
        
        public void UpdateMoney()
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
            _darkening.enabled = true;
            _winPopup.SetAward(award);
            _winPopup.Show();
            _winPopup.SetOnClicked(() =>
            {
                _winPopup.Hide(true, _huntingManager.Continue);
            });
        }

        public void Fail()
        {
            _darkening.enabled = true;
            _win = false;
            _failPopup.SetOnClicked(() =>
            {
                _failPopup.Hide(true, _huntingManager.Restart);
            });
            _failPopup.Show();
        }

        public void HidePopup(Action onEnd)
        {
            if (_win)
                _winPopup.Hide(true, onEnd);
            else
                _failPopup.Hide(true, onEnd);
        }
        
    }
}