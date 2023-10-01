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
        }

        private void SpawnPreyAndHunters(CamFollower camera)
        {
            var level = GC.LevelRepository.GetLevel(GC.PlayerData.LevelIndex);
            camera.CameraFlyDir = level.CameraFlyDir;
            _hunters = _huntPackSpawner.SpawnPack();
            _preyPack.Init(_spline);
            _preyPack.Idle();
            _preyPack.OnAllDead += Win;

            _hunters.Init(_preyPack, camera);
            _hunters.OnAllWasted += Loose;
            _hunters.IdleState();
            
            _preyPack.RunCameraAround(camera, () =>
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