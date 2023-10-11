using System;
using UnityEngine;

namespace Game.Hunting
{
    public class MergeItemFish : MonoBehaviour
    {
        [SerializeField] private SmallFishTank _fishTank;

        private void OnEnable()
        {
            _fishTank.Idle();
        }
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if(_fishTank == null)
                _fishTank = GetComponent<SmallFishTank>();
        }
#endif
    }
}