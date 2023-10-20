using System.Collections;
using Game.Merging;
using UnityEngine;
using Utils;

namespace Game.UI.Merging
{
    [DefaultExecutionOrder(1000)]
    public class SuperEggsPopupTimer : MonoBehaviour
    {
        [SerializeField] private SuperEggUI _superEggUI;
        private Coroutine _working;
        
        public void Hide()
        {
            StopWork();
            _superEggUI.Hide();
        }

        public void Show()
        {
            StopWork();
            _superEggUI.HideLabel();
            var stash = GC.ItemsStash;
            foreach (var egg in stash.SuperEggs)
            {
                if (egg.IsTicking)
                {
                    _superEggUI.Show(egg);
                    _working = StartCoroutine(Working(egg));
                    return;
                }
            }
            CLog.LogWHeader("EggsTimer", "No eggs are ticking", "w");
            _superEggUI.Hide();
        }

        private void StopWork()
        {
            if(_working != null)
                StopCoroutine(_working);
        }

        private IEnumerator Working(SuperEgg egg)
        {
            while (egg.IsTicking)
            {
                var time = egg.TimeLeft;
                time.CorrectToZero();
                if (time == TimerTime.Zero)
                {
                    if (SuperEggHelper.AlreadyAdded(egg.Item.item_id) == false)
                    {
                        egg.StopTicking();
                        GC.ItemsStash.Stash.AddItem(new MergeItem(egg.Item));                 
                    }
                    _superEggUI.Stop();
                    _superEggUI.ShowUnlocked();
                    yield break;
                }
                yield return null;
            }
        }
    }
}