using Game.Core;
using Game.Saving;
using Game.UI.Merging;
using UnityEngine;

namespace Game.Merging
{
    [DefaultExecutionOrder(1)]
    public class MergeTutorTestInitiator : MonoBehaviour
    {
        [SerializeField] private MergeManager _mergeManager;
        [SerializeField] private MergeUIPage _mergeUIPage;
        [SerializeField] private GCLocator _locator;
        [SerializeField] private SavedDataInitializer _dataInitializer;
        [Space(10)]
        [SerializeField] private PurchaseTutorial _purchaseTutor;
        [SerializeField] private MergeTutorial _mergeTutor;
        
        private void Awake()
        {
            _locator.InitContainer();
            _dataInitializer.InitSavedData();
            _mergeManager.Init();
            _mergeUIPage.Init(_mergeManager, _mergeManager.MergeInput);
        }

        [ContextMenu("Purchase Tutorial")]
        public void PlayBuyTutor()
        {
            _purchaseTutor.BeginTutorial(() => {});
        }

        [ContextMenu("Merge Tutorial")]
        public void PlayMergeTutor()
        {                
            _mergeTutor.BeginTutorial(() => {});
        }
    }
}