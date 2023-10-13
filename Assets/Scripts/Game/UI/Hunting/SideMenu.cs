using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hunting.UI
{
    public class SideMenu : MonoBehaviour
    {
        [SerializeField] private Button _enterButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _replayButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private GameObject _block;
        [SerializeField] private HuntingManager _huntingManager;

        private void Start()
        {
            _enterButton.onClick.AddListener(Enter);
            _closeButton.onClick.AddListener(Close);
            _exitButton.onClick.AddListener(Exit);
            _replayButton.onClick.AddListener(Replay);
        }

        private void Replay()
        {
            _huntingManager.ReplayLevel();
        }

        private void Exit()
        {
            _huntingManager.ExitToMerge();
        }

        private void Close()
        {
            _block.SetActive(false);
            Time.timeScale = 1f;
        }

        private void Enter()
        {
            Time.timeScale = 0f;
            _block.SetActive(true);
        }
        

        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if( _huntingManager == null)
                _huntingManager = FindObjectOfType<HuntingManager>();
            if (_block == null)
                _block = gameObject;
        }
#endif
        
    }
}