using System;
using Dreamteck.Splines;
using Game.Hunting.HuntCamera;
using Game.Hunting.UI;
using Game.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Game.Hunting
{
    [DefaultExecutionOrder(10)]
    public class HuntingManager : MonoBehaviour
    {
        
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private bool _doStart = true;
        [SerializeField] private bool _replayLevel = true;
        [SerializeField] private int _environment;
        [SerializeField] private SplineComputer _splineComputer;
        [SerializeField] private SplineComputer _splineComputerWater;
        [SerializeField] private CamFollower _camFollower;
        
        public void Init(IHuntUIPage page)
        {
            if (_doStart == false)
                return;
            GC.PlayerData.CurrentEnvironmentIndex = _environment;
            GC.SlowMotion.SetNormalTime();
            var index = GC.PlayerData.LevelTotal;
            var go = GC.LevelRepository.GetLevel(index).GetLevelPrefab();
            go = Instantiate(go, transform.position, transform.rotation);
            var level = go.GetComponent<ILevel>();
            
            level.Init(page, new MovementTracks(_splineComputer, _splineComputerWater, _moveSpeed), _camFollower);
            level.OnReplay += ReplayLevel;
            level.OnExit += ExitToMerge;
            level.OnContinue += Continue;
        }

        public void ExitToMerge()
        {
            CLog.LogWHeader("HuntManager", "RESTART", "y");
            if(GameState.SingleLevelMode)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
                GC.SceneSwitcher.OpenScene("Merge", (result) =>{});
        }

        public void ReplayLevel()
        {
            CLog.LogWHeader("HuntManager", "Replay this level", "y");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Continue()
        {
            CLog.LogWHeader("HuntManager", "Continue clicked", "g");
            if (GameState.SingleLevelMode && _replayLevel)
            {
                ReplayLevel();
                return;
            }
            GC.PlayerData.LevelTotal++;
            var map = GC.UIManager.WinLevelMap;
            map.SetOnContinue(MoveToMerge);
            map.MoveToLevel(GC.PlayerData.LevelTotal);
        }

        private void MoveToMerge()
        {
            GC.SceneSwitcher.OpenScene("Merge", (result) =>{});   
            
        }
     
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Debug.Break();
            else if (Input.GetKeyDown(KeyCode.R))
                ExitToMerge();
            else if (Input.GetKeyDown(KeyCode.S))
            {
                var scale = Time.timeScale;
                if (Math.Abs(scale - 1) < .1f)
                {
                    Time.timeScale = 2f;
                    Time.fixedDeltaTime = 1 / 100f;
                }
                else
                {
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = 1 / 50f;
                }
            }
        }
#endif
    }
}