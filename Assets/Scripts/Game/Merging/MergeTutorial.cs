using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Utils;
using Game.Levels;
using Game.UI;
using Game.UI.Merging;
using UnityEngine;
using GC = Game.Core.GC;

namespace Game.Merging
{
    public class MergeTutorial : Tutorial
    {
        [SerializeField] private float _mergeHandMoveTime = 1f;
        [Space(10)] 
        [SerializeField] private MergeItemSO _itemToMerge;
        [SerializeField] private ShopTutorial _shopTutorial;
        [SerializeField] private GroupGridBuilder _gridBuilder;
        [SerializeField] private MergeClassesSwitcher _mergeClasses;
        [SerializeField] private MergeInput _mergeInput;
        private bool _shopClicked;
        private List<MergeItem> _items = new List<MergeItem>();

        
        
        private void Awake()
        {
            _tutorBlock.SetActive(false);
        }
        
        public override void BeginTutorial(Action onCompleted)
        {
            CLog.LogWHeader("MergeTutorial", "Began", "r", "w");
            SendAnalytics();
            _mergeInput.Deactivate();
            _shopTutorial.BeginTutorial(DelayedStart);
        }

        private void SendAnalytics()
        {
            AnalyticsEvents.OnTutorial("03_shop_merge");   
        }
        
        private void DelayedStart()
        {
            _buttonsBlocker.None();
            StartCoroutine(SkipFrames(_waitFramesCount, BeginMergeTutor));
        }

        private void BeginMergeTutor()
        {
            CLog.LogWHeader("MergeTutorial", "Merge phase", "r", "w");
            var id = _itemToMerge.Item.item_id;
            var mergeClassUI = _mergeClasses.ShowFirstWithItems();
            var p1 = mergeClassUI.GetItemUI(id).transform.position;
            _spotlight1.SetPosition(p1);
            _spotlight1.Show();
            
            var p2 = MergeToPoint(id);
            _spotlight2.SetPosition(p2);
            _spotlight2.Show();
            Hand.MoveFromTo(p1, p2, _mergeHandMoveTime);
            StopAllCoroutines();
            _items = GetItems();
            StartCoroutine(WaitForUser());
        }

        private Vector3 MergeToPoint(string id)
        {
            var cell = GetCell(id);
            var pos = cell.GetItemView().GetModelPosition();
            pos = Camera.main.WorldToScreenPoint(pos);
            return pos;
        }

        private IGroupCellView GetCell(string id)
        {
            var cells = _gridBuilder.GetSpawnedCells();
            foreach (var row in cells)
            {
                foreach (var cell in row)
                {
                    if (cell.IsFree)
                        continue;
                    if (cell.GetItem().item_id == id)
                    {
                        return cell;
                    }
                }
            }
            return null;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator WaitForUser()
        {
            yield return null;
            while (true)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    yield return null;
                    yield return null;
                    yield return null;
                    var items = GetItems();
                    if(CompareItems(items, _items) == false)
                        FinishTutorials();
                }
                yield return null;
            }
        }

        private List<MergeItem> GetItems()
        {
            var items = new List<MergeItem>();
        
            var row = _gridBuilder.GetSpawnedCells()[0];
            foreach (var cell in row)
            {
                if(cell.IsFree) 
                    continue;
                items.Add(cell.GetItem());
            }
            return items;
        }

        private bool CompareItems(List<MergeItem> list_one, List<MergeItem> list_two)
        {
            if (list_one.Count != list_two.Count)
                return false;
            for (var i = 0; i < list_one.Count; i++)
            {
                if (list_one[i] != list_two[i])
                    return false;
            }
            return true;
        }
        
        private void FinishTutorials()
        {
            CLog.LogWHeader("MergeTutorial", "Finished", "r", "w");
            _buttonsBlocker.All();
            _spotlight1.HideAll();
            _spotlight2.HideAll();
            Hand.Hide();
            GC.PlayerData.TutorPlayed_Merge = true;
        }
    }
}