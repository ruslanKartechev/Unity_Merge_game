using Game.UI.Merging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Merging
{
    public class MergeInitiator : MonoBehaviour
    {
        [SerializeField] private MergeManager _mergeManager;
        [SerializeField] private MergeUIPage _mergeUIPage;
        [SerializeField] private Tutorial _tutorial;
        
        private void Start()
        {
            if (GC.PlayerData == null)
            {
                Debug.Log($"Container references not found! Game Should Start From ''Start'' SCENE ");
                SceneManager.LoadScene("Start");
                return;
            }
            
            _mergeManager.Init();
            _mergeUIPage.Init(_mergeManager, _mergeManager.MergeInput);
            if(GC.PlayerData.TutorPlayed_Merge == false)
                _tutorial.BeginTutorial(OnTutorCompleted);
        }

        private void OnTutorCompleted()
        {
            Debug.Log("On Tutor Completed");
        }
    }
}