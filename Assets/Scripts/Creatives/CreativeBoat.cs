using System;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Creatives
{
    public class CreativeBoat : MonoBehaviour, IPrey
    {
        public event Action<IPrey> OnKilled;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private CarPassenger _passenger;
        [SerializeField] private PreySettings _settings;

        public void Init()
        {
            IsAvailableTarget = true;
            PreySettings = _settings;
        }

        public void OnPackRun()
        {
        }

        public void OnPackAttacked()
        {
        }

        public float GetReward()
        {
            return 100;
        }

        public PreySettings PreySettings { get; set; }
        public ICamFollowTarget CamTarget => _camFollowTarget;
        public bool IsAvailableTarget { get; private set; }
    }
}