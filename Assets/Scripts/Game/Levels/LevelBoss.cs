using System;
using System.Collections;
using Common.Utils;
using Dreamteck.Splines;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey;
using Game.Hunting.Prey.Interfaces;
using Game.Merging;
using Game.UI.Hunting;
using UnityEngine;
using GC = Game.Core.GC;

namespace Game.Levels
{
    public class LevelBoss : Level, ILevel, IPreyTriggerListener
    {
       [SerializeField] private CameraFlyOver _bossFreedCamera;
       [SerializeField] private SuperEgg _rewardEgg;
       [SerializeField] private float _winPopDelay = .5f;
        
       public void Init(IHuntUIPage ui, MovementTracks track, GameObject camera)
       {
            GC.SlowMotion.SetNormalTime();
            _uiPage = ui;
            _track = track;
            _camera = camera.GetComponent<ICamFollower>();
            GetComps();
            SpawnPreyAndHunters(camera);
            _rewardCalculator.Init(_preyPack, _uiPage);
            GC.Input.Disable();
            AnalyticsEvents.OnStarted(AnalyticsEvents.normal);

        }
       
       public void OnAttacked()
       {
           _preyPack.RunAttacked();
       }
       
       private void BeginChase()
       {
           _hunters.BeginChase();
       }
       
        private void SpawnPreyAndHunters(GameObject camera)
        {
            var levelSettings = GC.LevelRepository.GetLevel(GC.PlayerData.LevelIndex);
            _hunters = _huntPackSpawner.SpawnPack(_track);
            _preyPack.Init(_track, levelSettings);
            _preyPack.Idle();
            _preyPack.OnAllDead += OnAllDead;
            _preyPack.OnBeganMoving += BeginChase;

            _hunters.OnAllWasted += Fail;
            _hunters.Init(_preyPack, _uiPage.InputButton, camera,_track);
            _hunters.IdleState();
            
            _preyPack.RunCameraAround(camera, () =>
            {
                GC.Input.Enable();
                _hunters.FocusCamera();
                _hunters.AllowAttack();
            });
        }

        private void OnAllDead()
        {
            _hunters.Win();
            GC.Input.Disable();
            GC.SlowMotion.SetNormalTime();
            _bossFreedCamera.RunCamera(_camera, Win);
        }

        private void Win()
        {
            CLog.LogWHeader("HuntManager", "Hunt WIN", "w");
            if (_isCompleted)
                return;
            _isCompleted = true;
            StartCoroutine(Winnning());
            AnalyticsEvents.OnFinished(AnalyticsEvents.boss);
        }

        private IEnumerator Winnning()
        {
            _rewardEgg.StartTicking();
            yield return null;
            _rewardCalculator.ApplyReward();
            _levelUIController.WinBoss(_rewardCalculator.TotalReward, _rewardEgg, RaiseOnContinue);
        }
        
        private void Fail()
        {
            if (_isCompleted)
                return;
            CLog.LogWHeader("HuntManager", "Hunt lost", "w");
            GC.Input.Disable();
            GC.SlowMotion.SetNormalTime();
            _isCompleted = true;
            _rewardCalculator.ApplyReward();
            StartCoroutine(DelayedCall(FinalLoose, _completeDelay));
        }

        private void FinalLoose()
        {
            AnalyticsEvents.OnFailed(AnalyticsEvents.boss);
            _levelUIController.Loose(_rewardCalculator.TotalReward, RaiseOnReplay, RaiseOnExit);
        }
        
        
        
        #if UNITY_EDITOR
        private bool _finished;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (_finished)
                    return;
                _finished = true;
                Win();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_finished)
                    return;
                _finished = true;
                Fail();
            }
        }
#endif
 
    }
}