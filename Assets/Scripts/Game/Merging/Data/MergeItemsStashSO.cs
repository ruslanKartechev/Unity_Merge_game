using System;
using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(MergeItemsStashSO), fileName = nameof(MergeItemsStashSO), order = 10)]
    public class MergeItemsStashSO : ScriptableObject
    {
        [SerializeField] private MergeItemsStash _initialStash;
        [NonSerialized] private MergeItemsStash _currentStash;
        
        public MergeItemsStash Stash
        {
            get
            {
                if (_currentStash == null)
                {
                    Debug.Log($"Null stash, creating copy of initial");
                    _currentStash = new MergeItemsStash(_initialStash);
                    _currentStash.Init();
                }
                return _currentStash;
            }
            set
            {
                _currentStash = value;
                _currentStash.Init();
            }
        }
    }
}