using DG.Tweening;
using Game.Merging;
using Game.UI.Elements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergingPage : MonoBehaviour, IMergingPage
    {
        private const float PlayButtonShowTime = 0.25f;
        
        [SerializeField] private MergeManager _mergeManager;
        [SerializeField] private MoneyDisplayUI _moneyDisplay;
        [SerializeField] private LevelDisplay _levelDisplay;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private TextMeshProUGUI _pruchaseCostText;
        [SerializeField] private PunchAnimator _buyPunchAnimator;
        [SerializeField] private BouncyPrompt _bouncyPrompt;
        
        private void Start()
        {
            _moneyDisplay.UpdateCount(false);
            _levelDisplay.SetLevel( Container.PlayerData.LevelTotal+1);
            _startButton.onClick.AddListener(OnStartButton);
            _buyButton.onClick.AddListener(OnBuyButton);
            _mergeManager.Init(this);
            _bouncyPrompt.Show();
        }

        public void SetPurchaseCost(float cost)
        {
            _pruchaseCostText.text = $"{cost}";
        }

        public void UpdateMoney() => _moneyDisplay.UpdateCount();

        public void ShowPlayButton(bool animated)
        {
            if (!animated)
            {
                _startButton.gameObject.SetActive(true);
                _startButton.transform.localScale = Vector3.one;
                return;
            }
            var tr = _startButton.transform;
            _startButton.gameObject.SetActive(true);
            tr.localScale = Vector3.zero;
            tr.DOScale(Vector3.one, PlayButtonShowTime).SetEase(Ease.InBounce);
        }

        public void HidePlayButton()
        {
            _startButton.gameObject.SetActive(false);
        }

        private void OnStartButton()
        {
            _mergeManager.OnPlayButton();
        }
        
        private void OnBuyButton()
        {
            _mergeManager.OnPurchase();
            // _buyPunchAnimator.PunchAnimate();
        }
    }
}