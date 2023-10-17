using System;
using System.Collections;
using Dreamteck.Splines;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.UI;
using Game.UI;
using UnityEngine;
using Utils;

namespace Game.Levels
{
    public class TutorialLevel : Level, ILevel, IPreyTriggerListener
    {
        [SerializeField] private LevelTutorialUI _ui;   
        [SerializeField] private float _tutorStartDelay = 1f;
        private Coroutine _tutoring;

        
        public void Init(IHuntUIPage ui, MovementTracks track, CamFollower camera)
        {
            _analyticsEvents = new AnalyticsEvents();
            _analyticsEvents.OnStarted(AnalyticsEvents.normal);

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
            // GC.Input.Enable();
            _preyPack.RunAttacked();
            // _hunters.AllowAttack();
            // _hunters.BeginChase();
            GC.PlayerData.TutorPlayed_Attack = true;
        }

        private void BeginChase()
        {
            _hunters.BeginChase();
        }
        
        private void SpawnPreyAndHunters(CamFollower camera)
        {
            var levelSettings = GC.LevelRepository.GetLevel(GC.PlayerData.LevelIndex);
            camera.CameraFlyDir = levelSettings.CameraFlyDir;
            _hunters = _huntPackSpawner.SpawnPack(_track);
            _preyPack.Init(_track, levelSettings);
            _preyPack.Idle();
            _preyPack.OnAllDead += Win;
            _preyPack.OnBeganMoving += BeginChase;

            _hunters.Init(_preyPack, _uiPage.InputButton, camera, _track);
            _hunters.OnAllWasted += Loose;
            _hunters.IdleState();
            
            if (GC.PlayerData.TutorPlayed_Attack)
            {
                _preyPack.RunCameraAround(camera, () =>
                {
                    StartCoroutine(AllowAttack());
                });            
            }
            else
            {
                _preyPack.RunCameraAround(camera, BeginTutor);
                _analyticsEvents.OnTutorial("01_aim_attack");
            }
        }

        private void BeginTutor()
        {
            StopTutor();
            _tutoring = StartCoroutine(Tutorial());
        }
        
        private void StopTutor()
        {
             if(_tutoring != null)
                 StopCoroutine(_tutoring);
             _ui.HideAll();
        }
        
        private IEnumerator AllowAttack()
        {
            ShowPower();
            _hunters.FocusCamera();
            yield return new WaitForSeconds(0.4f);
            GC.Input.Enable();
            _hunters.AllowAttack();    
        }
        
        private IEnumerator Tutorial()
        {
            _hunters.FocusCamera();
            yield return new WaitForSeconds(_tutorStartDelay);
            _ui.BeginAimTutor();
            GC.Input.Enable();
            _hunters.AllowAttack();   
            var loop = true;
            while (loop)
            {
                if (GC.Input.IsDown())
                {
                    _ui.HideAll();
                    loop = false;
                }
                yield return null;
            }
            _ui.BeginJumpTutor();

            loop = true;
            while (loop)
            {
                if (GC.Input.IsUp())
                {
                    _ui.HideAll();
                    loop = false;
                    yield return null;
                    GC.Input.Disable();
                }
                yield return null;
            }            
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
            // _uiPage.Win(_rewardCalculator.TotalReward);
            _levelUIController.Win(_rewardCalculator.TotalReward, RaiseOnContinue);
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
            _levelUIController.Loose(0f, RaiseOnReplay, RaiseOnExit);
        }
        

    }
}