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
    public class TutorialLevel : MonoBehaviour, ILevel, IPreyTriggerListener
    {
        [SerializeField] private LevelTutorialUI _ui;   
        
        [Header("Child 0 = Hunters\nChild 1 = Prey Pack")]
        [SerializeField] private float _completeDelay = 1f;
        [SerializeField] private float _tutorStartDelay = 1f;
        
        private CamFollower _camera;
        private SplineComputer _track;
        
        private IHuntPackSpawner _huntPackSpawner;
        private IHuntUIPage _uiPage;
        private IPreyPack _preyPack;
        private IHunterPack _hunters;
        private IRewardCalculator _rewardCalculator;
        
        private bool _isCompleted;
        private SplineComputer _spline;
        private Coroutine _tutoring;

        
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

            GC.Input.Disable();
            SpawnPreyAndHunters(camera);
            // if(!DebugSettings.SingleLevelMode)
            //     LoadingCurtain.Open(() =>{ });

            #region Reward
            _rewardCalculator = gameObject.GetComponent<IRewardCalculator>();
            _rewardCalculator.Init(_preyPack, _uiPage);
            #endregion
        }
        
        public void OnAttacked()
        {
            GC.Input.Enable();
            _preyPack.RunAttacked();
            _hunters.AllowAttack();
            _hunters.BeginChase();
            GC.PlayerData.TutorPlayed_Attack = true;
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
            if (GC.PlayerData.TutorPlayed_Attack)
            {
                _preyPack.RunCameraAround(camera, () =>
                {
                    StartCoroutine(AllowAttack());
                });            
            }
            else
                _preyPack.RunCameraAround(camera, BeginTutor);
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
            AllowAttack();
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
            StopTutor();
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
        }
                 
        private void Loose()
        {
            if (_isCompleted)
                return;
            StopTutor();
            CLog.LogWHeader("HuntManager", "Hunt lost", "w");
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

     

    }
}