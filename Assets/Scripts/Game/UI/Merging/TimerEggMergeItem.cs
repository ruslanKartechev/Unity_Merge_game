using System;
using System.Collections;
using Common;
using Game.Merging;
using TMPro;
using UnityEngine;
using GC = Game.Core.GC;

namespace Game.UI.Merging
{
    public class TimerEggMergeItem : MonoBehaviour
    {
        public event Action OnUnlocked;
        [Space(15)]   
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _itemName;
        
        public SuperEgg Egg { get; set; }
        
        public void Show(SuperEgg superEgg)
        {
            Egg = superEgg;
            _itemName.text = GC.ItemViews.GetDescription(superEgg.Item.item_id).ItemName;
            StartCoroutine(Working(superEgg));
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            if (Egg != null)
                StartCoroutine(Working(Egg));
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
                    OnUnlocked?.Invoke();
                    gameObject.SetActive(false);
                    yield break;
                }
                _text.text = time.TimeAsString;
                yield return null;
            }
        }
    }
}