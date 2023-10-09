using System;
using System.Collections;
using Common;
using Game.Levels;
using Game.UI;
using Game.UI.Merging;
using UnityEngine;

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
        private int _activeGroupCount = 0;
        private bool _shopClicked;

        private void Awake()
        {
            _tutorBlock.SetActive(false);
        }
        
        public override void BeginTutorial(Action onCompleted)
        {
            var analytics = new AnalyticsEvents();
            analytics.OnTutorial("02_shop_merge");
            _shopTutorial.BeginTutorial(DelayedStart);
        }

        private void DelayedStart()
        {
            StartCoroutine(SkipFrames(_waitFramesCount, BeginPlaceTutor));
        }

        private void BeginPlaceTutor()
        {
            Debug.Log("[Tutor] Merge Tutor began");
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
            _spotlight1.HideAll();
            _spotlight2.HideAll();
            Hand.Hide();
            GC.PlayerData.TutorPlayed_Merge = true;
            Debug.Log($"Merge Tutorial Finished");
        }
        

    }
}