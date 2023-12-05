using System;
using Common.Utils;
using Dreamteck.Splines;
using Game.Core;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Levels;
using Game.UI.Hunting;
using UnityEngine;
using UnityEngine.SceneManagement;
using GC = Game.Core.GC;

namespace Creatives
{
    public class CreativesHuntManager : MonoBehaviour
    {
        [SerializeField] private GameObject _level;
        [SerializeField] private GameObject _uiGo;
        [SerializeField] private GameObject _camera;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private bool _replayLevel = true;
        [SerializeField] private int _environment;
        [SerializeField] private SplineComputer _splineComputer;
        [SerializeField] private SplineComputer _splineComputerWater;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            Application.targetFrameRate = 60;
            var page = _uiGo.GetComponent<IHuntUIPage>();
            GC.PlayerData.CurrentEnvironmentIndex = _environment;
            GC.SlowMotion.SetNormalTime();
            var level = _level.GetComponent<ILevel>();
            level.Init(page, new MovementTracks(_splineComputer, _splineComputerWater, _moveSpeed), _camera);
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
            {
                AnalyticsEvents.OnExited(AnalyticsEvents.normal);
                GC.SceneSwitcher.OpenScene("Merge", (result) =>{});
            }
        }

        public void ReplayLevel()
        {
            CLog.LogWHeader("HuntManager", "Replay this level", "y");
            AnalyticsEvents.OnRestart(AnalyticsEvents.normal);
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
            GC.PlayerData.LevelIndex++;
            GC.PlayerData.LevelTotal++;
            // StartCoroutine(DelayedWin());
            // var map = GC.UIManager.WinLevelMap;
            // map.SetOnContinue(MoveToMerge);
            // map.MoveToLevel(GC.PlayerData.LevelTotal);
            MoveToMain();
        }

        private void MoveToMain()
        {
            GC.SceneSwitcher.OpenScene(SceneNames.MainScene, (result) =>{});
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