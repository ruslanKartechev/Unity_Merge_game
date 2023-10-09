using System.Collections;
using Common;
using Game.Levels;
using Game.UI;
using Game.UI.Merging;
using UnityEngine;

namespace Game.Merging
{
    public class PurchaseTutorial : Tutorial
    {
        [SerializeField] private float _minMoney = 100;
        [SerializeField] private ShopTutorial _shopTutorial;
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
        
        public void BeginTutorial()
        {
            var analytics = new AnalyticsEvents();
            analytics.OnTutorial("02_shop_merge");
            _shopTutorial.PlayTutor(DelayedStart);
        }

        private void DelayedStart()
        {
            StartCoroutine(SkipFrames(_waitFramesCount, BeginPlaceTutor));
        }

        private void BeginPlaceTutor()
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
            yield return null;
            while (true)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    yield return null;
                    
                    var count = GC.ActiveGroupSO.Group().ItemsCount;
                    if (count > _activeGroupCount)
                    {
                        FinishTutorials();
                        yield break;
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
        
        private void CorrectMoney()
        {
            if (GC.PlayerData.Money < _minMoney)
            {
                GC.PlayerData.Money = _minMoney;
                UIC.Money.UpdateCount();
            }            
        }
    }
}