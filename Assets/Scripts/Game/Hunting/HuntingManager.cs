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
        [SerializeField] private bool _doStart = true;
        [SerializeField] private bool _replayLevel = true;
        [SerializeField] private SplineComputer _splineComputer;
        [SerializeField] private CamFollower _camFollower;

        private IHuntUIPage _uiPage;
        public void Init(IHuntUIPage page)
        {
            if (_doStart == false)
                return;
            GC.SlowMotion.SetNormalTime();
            _uiPage = page;
            var index = GC.PlayerData.LevelIndex;
            var go = GC.LevelRepository.GetLevel(index).GetLevelPrefab();
            go = Instantiate(go);
            var level = go.GetComponent<ILevel>();
            level.Init(page, _splineComputer, _camFollower);
        }
        
        public void RestartFromMerge()
        {
            CLog.LogWHeader("HuntManager", "RESTART", "y");
            if(DebugSettings.SingleLevelMode)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
                GC.SceneSwitcher.OpenScene("Merge", (result) =>{});
        }
        
        public void ReplayLevel()
        {
            CLog.LogWHeader("HuntManager", "Replay this level", "y");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // called from UI
        public void Continue()
        {
            CLog.LogWHeader("HuntManager", "Continue clicked", "g");
            if (DebugSettings.SingleLevelMode && _replayLevel)
            {
                ReplayLevel();
                return;
            }
            GC.PlayerData.LevelIndex++;
            GC.PlayerData.LevelTotal++;
            GC.SceneSwitcher.OpenScene("Map", (result) =>{});
        }

     
#if UNITY_EDITOR
        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.W))
            //     Win();
            // if (Input.GetKeyDown(KeyCode.F))
            //     Loose();
            
            if (Input.GetKeyDown(KeyCode.Space))
                Debug.Break();
            else if (Input.GetKeyDown(KeyCode.R))
                RestartFromMerge();
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