using System.Collections;
using Game.Merging;
using UnityEngine;

namespace Game.UI.Merging
{
    [DefaultExecutionOrder(1000)]
    public class SuperEggsPopupTimer : MonoBehaviour
    {
        [SerializeField] private SuperEggUI _superEggUI;
        private Coroutine _working;
        
        private void Start()
        {
            _superEggUI.HideLabel();
            var stash = GC.ItemsStash;
            foreach (var egg in stash.SuperEggs)
            {
                if (egg.IsTicking)
                {
                    _superEggUI.Show(egg);
                    _working = StartCoroutine(Working(egg));
                    Debug.Log("SUPER EGG INITIATED");
                    return;
                }
            }
            Debug.Log("NO EGG IS TICKING");
            _superEggUI.Hide();
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