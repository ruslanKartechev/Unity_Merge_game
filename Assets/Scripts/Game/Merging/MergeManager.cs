using Common;
using Game.UI.Merging;
using UnityEngine;
using Utils;

namespace Game.Merging
{
    public class MergeManager : MonoBehaviour
    {
        [SerializeField] private CameraPoint _cameraPoint;
        [SerializeField] private MergingGridSpawner _gridSpawner;
        [SerializeField] private MergeGridRepository _mergeRepository;
        private IMergingPage _mergingPage;
        private IItemPurchaser _itemPurchaser;
        private IMergeInput _mergeInput;
        private IMergeItemSpawner _itemSpawner;
        private void Awake()
        {
            _mergeInput = GetComponent<IMergeInput>();
            _itemPurchaser = GetComponent<IItemPurchaser>();
            _itemSpawner = GetComponent<IMergeItemSpawner>();
        }

        public void SetUI(IMergingPage mergingPage)
        {
            _mergingPage = mergingPage;
        }
      
    }
}