﻿using System;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Creatives
{
    public class CreativeCar : MonoBehaviour, IPrey
    {
        public event Action<IPrey> OnKilled;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private CarWheelsController _wheels;
        [SerializeField] private CarPassenger _passenger;

        public void Init()
        {
            IsAvailableTarget = true;
        }

        public void OnPackRun()
        {
            _wheels.StartMoving();   
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