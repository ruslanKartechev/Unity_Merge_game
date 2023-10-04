using System;
using System.Collections;
using Dreamteck.Splines;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.UI;
using UnityEngine;
using Utils;

namespace Game.Levels
{

    public class LevelHunting : MonoBehaviour, ILevel, IPreyTriggerListener
    {
        [Header("Child 0 = Hunters\nChild 1 = Prey Pack")]
        [SerializeField] private float _completeDelay = 1f;
        
        private CamFollower _camera;
        private SplineComputer _track;
        
        private IHuntPackSpawner _huntPackSpawner;
        private IHuntUIPage _uiPage;
        private IPreyPack _preyPack;
        private IHunterPack _hunters;
        private IRewardCalculator _rewardCalculator;
        
        private bool _isCompleted;
        private SplineComputer _spline;
        private AnalyticsEvents _analyticsEvents;
        
        public void Init(IHuntUIPage ui, SplineComputer track, CamFollower camera)
        {
            GC.SlowMotion.SetNormalTime();
            _uiPage = ui;
            _spline = track;
            _huntPackSpawner = transform.GetChild(0).GetComponent<IHuntPackSpawner>();
            _preyPack = transform.GetChild(1).GetComponent<IPreyPack>();

            #region Warnings
            if(_huntPackSpawner== null)
                Debug.LogError($"HuntersPackSpawner is NULL on {gameObject.name}");
            if(_preyPack== null)
                Debug.LogError($"PreyPack is NULL on {gameObject.name}");
            #endregion

            SpawnPreyAndHunters(camera);
            GC.Input.Disable();
            // if(!DebugSettings.SingleLevelMode)
            //     LoadingCurtain.Open(() =>{ });

            #region Reward
            _rewardCalculator = gameObject.GetComponent<IRewardCalculator>();
            _rewardCalculator.Init(_preyPack, _uiPage);
            #endregion
            
            _analyticsEvents = new AnalyticsEvents();
            _analyticsEvents.OnStarted(AnalyticsEvents.normal);
        }

        public void OnAttacked()
        {
            GC.Input.Enable();
            _preyPack.RunAttacked();
            _hunters.AllowAttack();
            _hunters.BeginChase();
        }
        
        private void SpawnPreyAndHunters(CamFollower camera)
        {
            var levelSettings = GC.LevelRepository.GetLevel(GC.PlayerData.LevelIndex);
            camera.CameraFlyDir = levelSettings.CameraFlyDir;
            _hunters = _huntPackSpawner.SpawnPack();
            _preyPack.Init(_spline, levelSettings);
            _preyPack.Idle();
            _preyPack.OnAllDead += Win;

            _hunters.Init(_preyPack, camera);
            _hunters.IdleState();
            _hunters.OnAllWasted += Loose;
           
            _preyPack.RunCameraAround(camera, () =>
            {
                StartCoroutine(AllowAttack());
            });
        }
        
        
        private void Win()
        {
            if (_isCompleted)
                return;
            CLog.LogWHeader("HuntManager", "Hunt WIN", "w");
            GC.Input.Disable();
            GC.SlowMotion.SetNormalTime();
            _isCompleted = true;
            _rewardCalculator.ApplyReward();
            StartCoroutine(DelayedCall(() =>
            {
                _hunters.Win();
                _uiPage.Win(_rewardCalculator.TotalReward);
            }, _completeDelay));
         
            _analyticsEvents.OnWin(AnalyticsEvents.normal);
        }
                 
        private void Loose()
        {
            if (_isCompleted)
                return;
            CLog.LogWHeader("HuntManager", "Hunt lost", "w");
            _rewardCalculator.ResetReward();
            GC.Input.Disable();
            GC.SlowMotion.SetNormalTime();
            _isCompleted = true;
            StartCoroutine(DelayedCall(() =>
            {
                _uiPage.Fail();
            }, _completeDelay));

            _analyticsEvents.OnFailed(AnalyticsEvents.normal);
        }
         
        private IEnumerator DelayedCall(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
        
        private IEnumerator AllowAttack()
        {
            _hunters.FocusCamera();
            yield return new WaitForSeconds(0.4f);
            GC.Input.Enable();
            _hunters.AllowAttack();    
        }

        
    }
}