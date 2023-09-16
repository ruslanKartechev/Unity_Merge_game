using Common;
using Game.UI.Merging;
using UnityEngine;

namespace Game.Merging
{
    public class MergeManager : MonoBehaviour
    {
        [SerializeField] private CameraPoint _cameraPoint;
        [SerializeField] private GroupGridBuilder gridBuilder;
        [SerializeField] private ActiveGroupSO _mergeRepository;
        [SerializeField] private MergeInputUI _mergeInputUI;
        private IMergingPage _mergingPage;
        private IMergeInput _mergeInput;
        private IMergeItemSpawner _itemSpawner;
        private void GetComponents()
        {
            _mergeInput = GetComponent<IMergeInput>();
            _itemSpawner = GetComponent<IMergeItemSpawner>();
        }

        public void SetUI(IMergingPage mergingPage)
        {
            GetComponents();
            _mergingPage = mergingPage;
            gridBuilder.Spawn(_mergeRepository.GetSetup(), _itemSpawner);
            _mergeInput.Init(_mergingPage, _itemSpawner, _mergeInputUI);
            _mergeInput.Activate();
        }
      
    }
}