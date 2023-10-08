using System;
using System.Collections;
using Dreamteck.Splines;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.UI;
using UnityEngine;

namespace Game.Levels
{
    public abstract class Level : MonoBehaviour
    {
        protected const float PowerDisplayTime = 4f;
        protected const float CameraFocusTime = .44f;
        [Header("Child 0 = Hunters\nChild 1 = Prey Pack")]
        [SerializeField] protected float _completeDelay = 1f;
        
        protected CamFollower _camera;
        protected SplineComputer _track;
        protected AnalyticsEvents _analyticsEvents;
        
        protected IHuntPackSpawner _huntPackSpawner;
        protected IRewardCalculator _rewardCalculator;
        protected IPreyPack _preyPack;
        
        protected IHuntUIPage _uiPage;
        protected IHunterPack _hunters;
        protected bool _isCompleted;
        

        protected void GetComps()
        {
            _huntPackSpawner = transform.GetChild(0).GetComponent<IHuntPackSpawner>();
            _preyPack = transform.GetChild(1).GetComponent<IPreyPack>();
            _rewardCalculator = gameObject.GetComponent<IRewardCalculator>();

            #region Warnings
            if(_huntPackSpawner== null)
                Debug.LogError($"HuntersPackSpawner is NULL on {gameObject.name}");
            if(_preyPack== null)
                Debug.LogError($"PreyPack is NULL on {gameObject.name}");
            #endregion
        }

        protected void SetupAnalytics()
        {
            _analyticsEvents = new AnalyticsEvents();
            _analyticsEvents.OnStarted(AnalyticsEvents.normal);
        }
        
        protected IEnumerator DelayedCall(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
        
        protected void ShowPower()
        {
            _uiPage.ShowPower(_hunters.TotalPower(), _preyPack.TotalPower(), PowerDisplayTime);
        }

    }
}