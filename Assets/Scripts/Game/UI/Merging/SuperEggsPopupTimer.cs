﻿using System.Collections;
using Game.Merging;
using UnityEngine;

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
            Debug.Log("NO super eggs are ticking");
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
                egg.TimeLeft.CorrectToZero();
                if (egg.TimeLeft == TimerTime.Zero)
                {
                    GC.ItemsStash.Stash.AddItem(new MergeItem(egg.Item));
                    egg.StopTicking();
                    _superEggUI.Stop();
                    _superEggUI.ShowUnlocked();
                    yield break;
                }
                yield return null;
            }
        }
    }
}