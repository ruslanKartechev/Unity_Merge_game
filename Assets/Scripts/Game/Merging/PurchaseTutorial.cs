using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Game.Levels;
using Game.UI.Merging;
using UnityEngine;
using Utils;

namespace Game.Merging
{
    public class PurchaseTutorial : Tutorial
    {
        [SerializeField] private float _mergeHandMoveTime = 1f;
        [SerializeField] private ShopTutorial _shopTutorial;
        [SerializeField] private MergeClassesSwitcher _mergeClasses;
        [SerializeField] private MergeInput _mergeInput;
        [SerializeField] private GroupGridBuilder _gridBuilder;

        private int _activeGroupCount = 0;
        private bool _shopClicked;

        private void Awake()
        {
            _tutorBlock.SetActive(false);
        }
            
        public override void BeginTutorial(Action onCompleted)
        {
            CLog.LogWHeader("PurchaseTutorial", "Began", "r", "w");
            SendAnalytics();
            GC.PlayerData.TutorPlayed_Purchased = true;
            _mergeInput.Deactivate();
            _shopTutorial.BeginTutorial(DelayedStart);
        }

        private void SendAnalytics()
        {
            AnalyticsEvents.OnTutorial("02_shop_purchase");   
        }

        private void DelayedStart()
        {
            StartCoroutine(SkipFrames(_waitFramesCount, BeginPlaceTutor));
        }

        private void BeginPlaceTutor()
        {
            CLog.LogWHeader("PurchaseTutorial", "Placing stage", "r", "w");
            var mergeClassUI = _mergeClasses.ShowFirstWithItems();
            var p1 = mergeClassUI.GetFirstCellWithItem().transform.position;
            _spotlight1.SetPosition(p1);
            _spotlight1.Show();
            var p2 = GetDragToPos();
            _spotlight2.SetPosition(p2);
            _spotlight2.Show();
            Hand.MoveFromTo(p1, p2, _mergeHandMoveTime);
            _activeGroupCount = GC.ActiveGroupSO.Group().ItemsCount;
            // Debug.Log($"[Tutor] Start item count: {_activeGroupCount}");
            StopAllCoroutines();
            StartCoroutine(WaitingForItemToSpawn());
            _buttonsBlocker.None();
        }

        private Vector3 GetDragToPos()
        {
            foreach (var row in _gridBuilder.GetSpawnedCells())
            {
                var count = row.Count;
                for(var x = 0; x < count; x++)
                {
                    if(row[x].IsFree)
                        continue;
                    var ind = x;
                    if (x - 1 > 0)
                        ind = x - 1;
                    else if (x + 1 < count)
                        ind = x + 1;
                    return Camera.main.WorldToScreenPoint(row[ind].GetPosition());   
                }
            }
            return Vector3.back;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator WaitingForItemToSpawn()
        {
            var clickCount = 0;
            var startItems = GetItems();
            yield return null;
            while (true)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    clickCount++;
                    yield return null;
                    yield return null;
                    var items = GetItems();
                    if (!Compare(items, startItems) || clickCount >= 2)
                    {
                        FinishTutorials();
                        yield break;   
                    }
                }
                yield return null;
            }
        }

        private List<string> GetItems()
        {
            var row = GC.ActiveGroupSO.Group().GetRow(0);
            var list = new List<string>(row.CellsCount);
            for (var i = 0; i < row.CellsCount; i++)
            {
                var item = row.GetCell(i).Item;
                if(item != null)
                    list.Add(item.item_id);
            }
            // Debug.Log($"***** GET ITEMS COUNT: {list.Count}");
            return list;
        }

        private bool Compare(List<string> items1, List<string> items2)
        {
            // Debug.Log("COMPARING");
            if (items1.Count != items2.Count)
                return false;
            for (var i = 0; i < items1.Count; i++)
            {
                if (items1[i] != items2[i])
                {
                    // Debug.Log($"Item1 {items1[i]}, Item2 {items2[i]}");
                    return false;
                }
            }
            return true;
        }

        private void FinishTutorials()
        {
            CLog.LogWHeader("PurchaseTutorial", "Finished", "r", "w");
            _spotlight1.HideAll();
            _spotlight2.HideAll();
            Hand.Hide();
            _buttonsBlocker.All();
        }
        
    }
}