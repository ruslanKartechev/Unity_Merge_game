using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.Dev
{
    public class DevUI : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _mainBlock;
        [SerializeField] private Button _enterButton;
        [SerializeField] private Button _closeButton;
        [Space(10)]
        [SerializeField] private DevActions _dev;
        [Space(10)]
        [SerializeField] private Button _addMoney;
        [SerializeField] private Button _addCrystals;
        [Space(10)] 
        [SerializeField] private Slider _sensitivityX;
        [SerializeField] private Slider _sensitivityY;
        [SerializeField] private TextMeshProUGUI _sensTextX;
        [SerializeField] private TextMeshProUGUI _sensTextY;
        [Space(10)]
        [SerializeField] private Button _prevLevel;
        [SerializeField] private Button _nextLevel;
        private const float time_for_double_click = .15f;
        private int _clicksCount;

        private Coroutine _doubleClickCheck;
        
        
        private void Awake()
        {
            _mainBlock.SetActive(false);
            _enterButton.onClick.AddListener(Open);
            _closeButton.onClick.AddListener(Close);
            _addMoney.onClick.AddListener(() =>
            {
                _dev.AddMoney();
            });
            _addCrystals.onClick.AddListener(() =>
            {
                _dev.AddCrystals();
            });
            
            _prevLevel.onClick.AddListener(() =>
            {
                _dev.PrevLevel();
            });
            _nextLevel.onClick.AddListener(() =>
            {
                _dev.NextLevel();
            });
            // InitSens();
        }

        private void InitSens()
        {
            var sensX = _dev.GetMaxSensX();
            var sensY = _dev.GetMaxSensY();
            
            _sensitivityX.value = sensX;
            _sensitivityY.value = sensY;
            
            _sensTextX.text = $"{sensX:N3}";
            _sensTextY.text = $"{sensY:N3}";
            
            _sensitivityX.onValueChanged.AddListener(SetSensX);
            _sensitivityY.onValueChanged.AddListener(SetSensY);
        }

        private void SetSensX(float val)
        {
            _dev.SetMaxSensX(val);
            _sensTextX.text = $"{val:N3}";
        }
        
        private void SetSensY(float val)
        {
            _dev.SetMaxSensY(val);
            _sensTextY.text = $"{val:N3}";
        }

        public void Close()
        {
            _mainBlock.SetActive(false);
        }

        private const int ClicksToOpen = 1;
        public void Open()
        {
            _clicksCount++;
            CLog.LogWHeader("DEV", $"Open Button clicks: {_clicksCount}", "r");
            if (_clicksCount >= ClicksToOpen)
            {
                _clicksCount = 0;
                _canvas.enabled = true;
                _mainBlock.SetActive(true);
            }
            else
                StartDCC();
        }

        private void StartDCC()
        {
            StopDCC();
            _doubleClickCheck = StartCoroutine(DoubleClickCheck());
        }
        
        private void StopDCC()
        {
            if(_doubleClickCheck != null)
                StopCoroutine(_doubleClickCheck);
        }
        
        private IEnumerator DoubleClickCheck()
        {
            yield return new WaitForSecondsRealtime(time_for_double_click);
            _clicksCount = 0;
        }
    }
}