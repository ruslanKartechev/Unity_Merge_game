using System;
using System.Collections;
using Common;
using Game.Levels;
using Game.UI;
using Game.UI.Merging;
using Game.UI.Shop;
using UnityEngine;

namespace Game.Merging
{
    public class MergeTutorial : MonoBehaviour
    {
        [SerializeField] private float _minMoney = 100;
        [SerializeField] private float _floatBeginDelay = 0.33f;
        [SerializeField] private GameObject _tutorBlock;
        [SerializeField] private TutorSpotlightUI _spotlight1;
        [SerializeField] private TutorSpotlightUI _spotlight2;
        [Space(10)]
        [SerializeField] private MergeCanvasSwitcher _switcher;
        [SerializeField] private Transform _enterShopSpotlightPoint;
        [SerializeField] private float _enterShopSpotlightSize = 300;
        [Space(10)]
        [SerializeField] private Transform _eggHandPoint;
        [SerializeField] private Transform _eggSpotlightPoint;
        [SerializeField] private float _buyItemSpotlightSize = 600;
        [SerializeField] private PurchasedItemDisplay _shopItemDisplay;
        [Space(10)]
        [SerializeField] private float _exitShopSpotlightSize;
        [SerializeField] private Transform _exitShopSpotlightPoint;
        [SerializeField] private Transform _exitShopHandPoint;
        [Space(10)] 
        [SerializeField] private float _mergeHandMoveTime = 1f;
        [SerializeField] private Transform _mergeWorldSpotlightPos;
        [SerializeField] private MergeClassesSwitcher _mergeClasses;
        private int _activeGroupCount = 0;
        
        
        private bool _shopClicked;

        private void Awake()
        {
            _tutorBlock.SetActive(false);
        }

        private void CorrectMoney()
        {
            if (GC.PlayerData.Money < _minMoney)
            {
                GC.PlayerData.Money = _minMoney;
                UIC.Money.UpdateCount();
            }            
        }

        public void BeginTutorial()
        {
            var analytics = new AnalyticsEvents();
            analytics.OnTutorial("02_shop_merge");
            
            CorrectMoney();
            StartCoroutine(Working());
        }

        private IEnumerator Working()
        {   
            GC.Input.Disable();
            yield return new WaitForSeconds(_floatBeginDelay);
            ShowEnterShopTutor();
        }

        private IEnumerator Delayed(float delay, Action onEnd)
        {
            yield return new WaitForSeconds(delay);
            onEnd.Invoke();
        }
        
        private void ShowEnterShopTutor()
        {
            _tutorBlock.gameObject.SetActive(true);
            _spotlight1.Show();
            _spotlight1.SetSize(_enterShopSpotlightSize);
            Hand.ShowClickingAt(_enterShopSpotlightPoint.position);
            _spotlight1.SetPosition(_enterShopSpotlightPoint.position);
            _switcher.OnShop += () =>
            {
                StartCoroutine(Delayed(.1f, ShowShopTutor));   
            };
        }
        
        private void ShowShopTutor()
        {
            _switcher.OnShop -= ShowShopTutor;
            _spotlight1.SetSize(_buyItemSpotlightSize);
            _spotlight1.SetPosition(_eggSpotlightPoint.position);
            _spotlight1.Show();
            Hand.ShowClickingAt(_eggHandPoint.position);
            _shopItemDisplay.OnDisplayStarted += () =>
            {
                _spotlight1.Hide();
                Hand.Hide();
            };
            _shopItemDisplay.OnDisplayEnded += ShowExitShopTutor;
        }

        private void ShowExitShopTutor()
        {
            _spotlight1.SetSize(_exitShopSpotlightSize);
            _spotlight1.SetPosition(_exitShopSpotlightPoint.position);
            _spotlight1.Show();
            Hand.ShowClickingAt(_exitShopHandPoint.position);
            _switcher.OnMain += OnMain;

        }

        private void OnMain()
        {
            _switcher.OnMain -= OnMain;
            _spotlight1.Hide();
            Hand.Hide();
            _activeGroupCount = GC.ActiveGroupSO.Group().ItemsCount;
            Debug.Log($"STARTING Items count: {_activeGroupCount}");
            StartCoroutine(Delayed(0.1f, ShowMergeTutor));
        }

        private void ShowMergeTutor()
        {
            Debug.Log("[Tutor] Show Merge Tutor");
            var mergeClassUI = _mergeClasses.ShowFirstWithItems();
            var p1 = mergeClassUI.GetFirstCellWithItem().transform.position;
            _spotlight1.SetPosition(p1);
            _spotlight1.Show();
            var p2 = Camera.main.WorldToScreenPoint(_mergeWorldSpotlightPos.position);
            _spotlight2.SetPosition(p2);
            _spotlight2.Show();
            Hand.MoveFromTo(p1, p2, _mergeHandMoveTime);
            StopAllCoroutines();
            StartCoroutine(WaitingForItemToSpawn());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator WaitingForItemToSpawn()
        {
            for(var f = 0; f < 4; f++)
                yield return null;
            while (true)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    yield return null;
                    yield return null;
                    
                    var count = GC.ActiveGroupSO.Group().ItemsCount;
                    if (count > _activeGroupCount)
                    {
                        FinishTutorials();
                        yield break;
                    }
                    else
                    {
                        // ShowMergeTutor();   
                    }
                }
                yield return null;
            }
        }

        private void FinishTutorials()
        {
            _spotlight1.Hide();
            _spotlight2.Hide();
            Hand.Hide();
            GC.PlayerData.TutorPlayed_Merge = true;
            Debug.Log($"Merge Tutorial Finished");
        }
    }
}