using System;
using System.Collections;
using Game.Merging;
using TMPro;
using UnityEngine;

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
                egg.TimeLeft.CorrectToZero();
                if (egg.TimeLeft == TimerTime.Zero)
                {
                    GC.ItemsStash.Stash.AddItem(new MergeItem(egg.Item));
                    egg.StopTicking();
                    OnUnlocked?.Invoke();
                    gameObject.SetActive(false);
                    yield break;
                }
                _text.text = egg.TimeLeft.TimeAsString;
                yield return null;
            }
        }
    }
}