using Game.UI.Merging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Merging
{
    public class MergeInitiator : MonoBehaviour
    {
        [SerializeField] private MergeManager _mergeManager;
        [SerializeField] private MergeUIPage _mergeUIPage;
        [SerializeField] private PurchaseTutorial _purchaseTutorial;
        [SerializeField] private MergeTutorial _mergeTutorial;
        
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

            switch (GC.PlayerData.LevelTotal)
            {
                case 1:
                    if(GC.PlayerData.TutorPlayed_Purchased == false)
                        _purchaseTutorial.BeginTutorial(OnTutorCompleted);
                    break;
                case 2:
                    if(GC.PlayerData.TutorPlayed_Merge == false)
                        _mergeTutorial.BeginTutorial(OnTutorCompleted);
                    break;
            }
        }

        private void OnTutorCompleted()
        {
            Debug.Log("On Tutor Completed");
        }
    }
}