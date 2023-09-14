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
        [SerializeField] private ActiveGroupSO _mergeRepository;
        private IMergingPage _mergingPage;
        private IMergeInput _mergeInput;
        private IMergeItemSpawner _itemSpawner;
        private void Awake()
        {
            _mergeInput = GetComponent<IMergeInput>();
            _itemSpawner = GetComponent<IMergeItemSpawner>();
        }

        public void SetUI(IMergingPage mergingPage)
        {
            _mergingPage = mergingPage;
            _gridSpawner.Spawn(_mergeRepository.GetSetup(), _itemSpawner);
        }
      
    }
}