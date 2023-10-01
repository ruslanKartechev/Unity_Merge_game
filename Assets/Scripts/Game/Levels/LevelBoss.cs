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
    public class LevelBoss : MonoBehaviour, ILevel, IPreyTriggerListener
    {
        [SerializeField] private float _completeDelay = 1f;
        
        private CamFollower _camera;
        private SplineComputer _track;
        
        private IPreySpawner _preySpawner;
        private IHuntPackSpawner _huntPackSpawner;
        private IHuntUIPage _uiPage;
        private IPreyPack _preyPack;
        private IHunterPack _hunters;
        private IRewardCalculator _rewardCalculator;
        
        private bool _isCompleted;
        private SplineComputer _spline;

        
        public void Init(IHuntUIPage ui, SplineComputer track, CamFollower camera)
        {
            _preySpawner = GetComponent<IPreySpawner>();
            _huntPackSpawner = GetComponent<IHuntPackSpawner>();
            
            GC.SlowMotion.SetNormalTime();
            SpawnPreyAndHunters(track, camera);
            _preyPack.OnAllDead += Win;
            GC.Input.Disable();
            // if(!DebugSettings.SingleLevelMode)
            //     LoadingCurtain.Open(() =>{ });
            _rewardCalculator = gameObject.GetComponent<IRewardCalculator>();
            _rewardCalculator.Init(_preyPack, _uiPage);
        }


        private void SpawnPreyAndHunters(SplineComputer spline, CamFollower camera)
        {
            var level = GC.LevelRepository.GetLevel(GC.PlayerData.LevelIndex);
            camera.CameraFlyDir = level.CameraFlyDir;
            var preyPack = _preySpawner.Spawn(spline, level);
            _hunters = _huntPackSpawner.SpawnPack();
            _preyPack = preyPack;
            preyPack.Idle();
                     
            _hunters.Init(preyPack, camera);
            _hunters.OnAllWasted += Loose;
            _hunters.IdleState();
            preyPack.RunCameraAround(camera, () =>
            {
                GC.Input.Enable();
                _hunters.FocusCamera();
                _hunters.AllowAttack();
            });
        }
        
        private void Win()
        {
            CLog.LogWHeader("HuntManager", "Hunt WIN", "w");
            if (_isCompleted)
                return;
            GC.Input.Disable();
            GC.SlowMotion.SetNormalTime();
            _isCompleted = true;
            _rewardCalculator.ApplyReward();
            StartCoroutine(DelayedCall(() =>
            {
                _hunters.Win();
                _uiPage.Win(_rewardCalculator.TotalReward);
            }, _completeDelay));
        }
                 
        private void Loose()
        {
            CLog.LogWHeader("HuntManager", "Hunt lost", "w");
            if (_isCompleted)
                return;
            _rewardCalculator.ResetReward();
            GC.Input.Disable();
            GC.SlowMotion.SetNormalTime();
            _isCompleted = true;
            StartCoroutine(DelayedCall(() =>
            {
                _uiPage.Fail();
            }, _completeDelay));
        }
         
        private IEnumerator DelayedCall(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
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