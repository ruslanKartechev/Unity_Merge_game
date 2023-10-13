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
    public class LevelHunting : Level, ILevel, IPreyTriggerListener
    {
        public void Init(IHuntUIPage ui, SplineComputer track, CamFollower camera)
        {
            GC.SlowMotion.SetNormalTime();
            _uiPage = ui;
            _track = track;
            GetComps();
            SpawnPreyAndHunters(camera);
            _rewardCalculator.Init(_preyPack, _uiPage);
            GC.Input.Disable();
            
            SetupAnalytics();
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
            _preyPack.Init(_track, levelSettings);
            _preyPack.Idle();
            _preyPack.OnAllDead += Win;

            _hunters.Init(_preyPack, _uiPage.InputButton, camera);
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
            StartCoroutine(DelayedCall(FinalWin, _completeDelay));
        }
        
        private void FinalWin()
        {
            _analyticsEvents.OnWin(AnalyticsEvents.normal);
            _hunters.Win();
            _levelUIController.Win(_rewardCalculator.TotalReward, RaiseContinue);
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
            StartCoroutine(DelayedCall(FinalLoose, _completeDelay));
        }

        private void FinalLoose()
        {
            _analyticsEvents.OnFailed(AnalyticsEvents.normal);
            _levelUIController.Loose(RaiseOnReplay, RaiseOnExit);
        }
        

        private IEnumerator AllowAttack()
        {
            ShowPower();
            _hunters.FocusCamera();
            yield return new WaitForSeconds(CameraFocusTime);
            GC.Input.Enable();
            _hunters.AllowAttack();    
        }


        
        #if UNITY_EDITOR
        private bool _finished;
        
        private void Update()
        {
            if (!_finished)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    _finished = true;
                    GC.Input.Disable();
                    GC.SlowMotion.SetNormalTime();
                    _isCompleted = true;
                    _rewardCalculator.ApplyReward();
                    FinalWin();
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    _finished = true;
                    Loose();
                }
            }
        }
#endif
    }
    
     
}