using Game.UI.Merging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Merging
{
    public class MergeInitiator : MonoBehaviour
    {
        [SerializeField] private MergeManager _mergeManager;
        [SerializeField] private PurchaseTutorial _tutorial;
        [SerializeField] private MergeUIPage _mergeUIPage;
        
        private void Start()
        {
            if (GC.PlayerData == null)
            {
                Debug.Log($"Container references not found! Game Should Start From ''Start'' SCENE ");
                SceneManager.LoadScene("Start");
                return;
            }
            
            if(GC.PlayerData.TutorPlayed_Merge == false)
                _tutorial.BeginTutorial();
            _mergeManager.Init();
            _mergeUIPage.Init(_mergeManager, _mergeManager.MergeInput);
        }
    }
}