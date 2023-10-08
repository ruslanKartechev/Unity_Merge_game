using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI.Map
{
    public class MapUIPage : MonoBehaviour
    {
        [SerializeField] private BottomButtons _buttons;
        [SerializeField] private UnityEngine.UI.Button _playButton;
        [SerializeField] private LevelsMap _map;
        private void Start()
        {
            _buttons.SetMap();
            _playButton.onClick.AddListener(Play);
            _buttons.OnCollection = MoveToCollection;
            _buttons.OnMain = MoveToMain;
            
            _map.ShowCurrentLevel();
            UIC.Crystals.UpdateCount();
            UIC.Money.UpdateCount();
        }

        private void Play()
        {
            SceneManager.LoadScene("Merge");
        }

        private void MoveToCollection()
        {
            Debug.Log("Collection button");   
        }

        private void MoveToMain()
        {
            SceneManager.LoadScene("Start");
        }
    }
}