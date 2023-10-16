using System.Collections;
using Dreamteck.Splines;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.UI;
using Game.Merging;
using UnityEngine;
using Utils;

namespace Game.Levels
{
    public class LevelBoss : Level, ILevel, IPreyTriggerListener
    {
       [SerializeField] private PreyPackCameraTrajectory _bossFreedCamera;
       [SerializeField] private SuperEgg _rewardEgg;
       [SerializeField] private float _winPopDelay = .5f;
        
        public void Init(IHuntUIPage ui, SplineComputer track, CamFollower camera)
        {
            GC.SlowMotion.SetNormalTime();
            _uiPage = ui;
            _track = track;
            _camera = camera;
            GetComps();
            SpawnPreyAndHunters(camera);
            _rewardCalculator.Init(_preyPack, _uiPage);
            GC.Input.Disable();
            
            SetupAnalytics();
        }

        private void SpawnPreyAndHunters(CamFollower camera)
        {
            var levelSettings = GC.LevelRepository.GetLevel(GC.PlayerData.LevelIndex);
            camera.CameraFlyDir = levelSettings.CameraFlyDir;
            _hunters = _huntPackSpawner.SpawnPack();
            _preyPack.Init(_track, levelSettings);
            _preyPack.Idle();
            _preyPack.OnAllDead += OnAllDead;

            _hunters.OnAllWasted += Loose;
            _hunters.Init(_preyPack, _uiPage.InputButton, camera);
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
            _camera.AllowFollowTargets = false;
            _bossFreedCamera.RunCamera(_camera, Win);
        }

        private void Win()
        {
            CLog.LogWHeader("HuntManager", "Hunt WIN", "w");
            if (_isCompleted)
                return;
            _isCompleted = true;
            StartCoroutine(Winnning());
            
            _analyticsEvents.OnWin(AnalyticsEvents.boss);
        }

        private IEnumerator Winnning()
        {
            _rewardEgg.StartTicking();
            yield return null;
            _uiPage.SuperEggUI.Show(_rewardEgg);
            yield return new WaitForSeconds(_winPopDelay);
            _uiPage.SuperEggUI.MoveDown();
            yield return null;
            _rewardCalculator.ApplyReward();
            _levelUIController.Win(_rewardCalculator.TotalReward, RaiseOnExit);
        }
        
        private void Loose()
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
            _analyticsEvents.OnFailed(AnalyticsEvents.normal);
            _levelUIController.Loose(_rewardCalculator.TotalReward, RaiseOnReplay, RaiseOnExit);
        }
        
        public void OnAttacked()
        {
            GC.Input.Enable();
            _preyPack.RunAttacked();
            _hunters.AllowAttack();
            _hunters.BeginChase();
        }
    }
}