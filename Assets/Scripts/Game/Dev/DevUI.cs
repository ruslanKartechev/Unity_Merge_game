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
        [SerializeField] private Slider _sensitivitySlider;
        [SerializeField] private TextMeshProUGUI _sensText;
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
            InitSens();
            _sensitivitySlider.onValueChanged.AddListener(SetSens);
        }

        private void InitSens()
        {
            var sensitivity = _dev.GetSensitivity();
            _sensitivitySlider.value = sensitivity;
            _sensText.text = $"{sensitivity:N3}";
        }

        private void SetSens(float val)
        {
            _dev.SetSensitivity(val);
            _sensText.text = $"{val:N3}";
        }

        public void Close()
        {
            _mainBlock.SetActive(false);
        }

        public void Open()
        {
            _clicksCount++;
            CLog.LogWHeader("DEV", $"Open Button clicks: {_clicksCount}", "r");
            if (_clicksCount == 2)
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