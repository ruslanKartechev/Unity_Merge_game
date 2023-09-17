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
        private IMergeInput _mergeInput;

        public IMergeInput MergeInput => _mergeInput;
        
        private void GetComponents()
        {
            _mergeInput = GetComponent<IMergeInput>();
        }
        
        public void Init()
        {
            GetComponents();
            gridBuilder.Spawn(_mergeRepository.GetSetup());
        }
      
    }
}