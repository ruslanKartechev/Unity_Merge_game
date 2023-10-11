using System;
using System.Collections;
using Common;
using Game.Levels;
using Game.UI.Merging;
using UnityEngine;
using Utils;

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
        private int _maxLevel = 0;
        private bool _shopClicked;

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
            try
            {
                var analytics = new AnalyticsEvents();
                analytics.OnTutorial("03_shop_merge");   
            }
            catch (System.Exception ex)
            {
                Debug.Log($"Exception: {ex.Message}\n{ex.StackTrace}");
            }
        }
        
        private void DelayedStart()
        {
            _buttonsBlocker.None();
            StartCoroutine(SkipFrames(_waitFramesCount, BeginMergeTutor));
        }

        private void BeginMergeTutor()
        {
            CLog.LogWHeader("MergeTutorial", "Merge phase", "r", "w");
            _maxLevel = GetMaxLevel();
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
            StartCoroutine(WaitingForItemToSpawn());
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
        private IEnumerator WaitingForItemToSpawn()
        {
            yield return null;
            while (true)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    yield return null;
                    yield return null;
                    yield return null;
                    var maxLvl = GetMaxLevel();
                    Debug.Log($"[Tutor] MaxLvl: {maxLvl}, Prev Max lvl: {_maxLevel}");
                    if (maxLvl > _maxLevel)
                    {
                        FinishTutorials();
                        yield break;
                    }
                }
                yield return null;
            }
        }

        private int GetMaxLevel()
        {
            var row = _gridBuilder.GetSpawnedCells()[0];
            var maxLvl = -1;
            foreach (var cell in row)
            {
                if(cell.IsFree) 
                    continue;
                var lvl = cell.GetItem().level;
                if (lvl > maxLvl)
                    maxLvl = lvl;
            }
            return maxLvl;
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