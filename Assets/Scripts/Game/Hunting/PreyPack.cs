using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using Game.Hunting.HuntCamera;
using UnityEngine;
using Utils;

namespace Game.Hunting
{
    public interface IPreyPack
    {
        event Action OnAllDead;
        event Action<IPrey> OnPreyKilled;
        
        IPrey GetClosestPrey(Vector3 position);
        void Activate();
        void Init(SplineComputer spline);
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        ICamFollowTarget GetCameraPoint();

    }
    
    public class PreyPack : MonoBehaviour, IPreyPack
    {
        public event Action OnAllDead;
        public event Action<IPrey> OnPreyKilled;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Transform _movable;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private List<Prey> _prey;

        private IPreyPackMover _mover;
        private HashSet<IPrey> _preyAlive;

        public ICamFollowTarget GetCameraPoint() => _camFollowTarget;

        
        public void Init(SplineComputer spline)
        {
            _mover = gameObject.GetComponent<IPreyPackMover>();
            _mover.Init(_moveSpeed, spline);
        }

        public Vector3 Position => _movable.position;
        
        public Quaternion Rotation => _movable.rotation;

        private void Awake()
        {
            Debug.Log("awake");
            _preyAlive = new HashSet<IPrey>(_prey.Count);
            foreach (var pp in _prey)
            {
                pp.Init();
                _preyAlive.Add(pp);
                pp.OnKilled += OnKilled;
            }
        }

        public void Activate()
        {
            Debug.Log("activate");
            _mover.BeginMoving();
            foreach (var prey in _preyAlive)
                prey.Activate();
        }

        public IPrey GetClosestPrey(Vector3 position)
        {
            var shortestD2 = float.MaxValue;
            IPrey result = null;
            foreach (var prey in _preyAlive)
            {
                var d2 = (prey.GetPosition() - position).sqrMagnitude;
                if (d2 < shortestD2)
                {
                    shortestD2 = d2;
                    result = prey;
                }
            }
            return result;
        }

        private void OnKilled(IPrey prey)
        {
            _preyAlive.Remove(prey);
            OnPreyKilled?.Invoke(prey);
            if (_preyAlive.Count == 0)
            {
                CLog.LogWHeader("PreyPack", "On prey killed", "g", "w");
                OnAllDead?.Invoke();
            }
        }
    }
}