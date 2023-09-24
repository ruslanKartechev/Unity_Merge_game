using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Game.Hunting.HuntCamera;
using UnityEngine;
using Utils;

namespace Game.Hunting
{
    public class PreyPack : MonoBehaviour, IPreyPack
    {
        public event Action OnAllDead;
        public event Action<IPrey> OnPreyKilled;
        public event Action OnPreyChaseBegin;

        [SerializeField] private float _surprisedTime = 1f;
        [Space(10)]
        [SerializeField] private Transform _movable;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private CamFollowTarget _attackCamTarget;
        [SerializeField] private PreyPackCameraTrajectory _preyPackCamera;
        [SerializeField] private List<Prey> _prey;

        private IPreyPackMover _mover;
        private HashSet<IPrey> _preyAlive;

        public void Init(SplineComputer spline)
        {
            _mover = _movable.GetComponent<IPreyPackMover>();
            _mover.Init(spline);
        }

        public Vector3 Position => _movable.position;
        
        public Quaternion Rotation => _movable.rotation;
        
        public Vector3 LocalToWorld(Vector3 position) => _movable.TransformPoint(position);
        
        public ICamFollowTarget CamTarget => _camFollowTarget;
        
        public ICamFollowTarget AttackCamTarget => _attackCamTarget;

        public int PreyCount => _preyAlive.Count;


        public void RunCameraAround(CamFollower cam, Action returnCamera)
        {
            if (_preyPackCamera == null)
            {
                returnCamera?.Invoke();
                return;
            }
            _preyPackCamera.RunCamera(cam, returnCamera);
        }
        
        public void Idle()
        {
            CLog.LogWHeader(nameof(PreyPack), "Prey pack IDLE", "b","w");
            _mover.StopMoving();
            foreach (var prey in _preyAlive)
                prey.IdleState();
        }
        
        public void Run()
        {
            CLog.LogWHeader(nameof(PreyPack), "Prey pack RUN", "b", "w");
            _mover.BeginMoving();
            foreach (var prey in _preyAlive)
                prey.RunState();
        }

        public void OnAttacked()
        {
            CLog.LogWHeader(nameof(PreyPack), "ON Attacked", "g");
            foreach (var prey in _preyAlive)
                prey.SurpriseToAttack();
            OnPreyChaseBegin?.Invoke();
            StartCoroutine(DelayedRun());
        }

        private IEnumerator DelayedRun()
        {
            yield return new WaitForSeconds(_surprisedTime);
            Run();
        }
        
        private void Awake()
        {
            _preyAlive = new HashSet<IPrey>(_prey.Count);
            foreach (var pp in _prey)
            {
                pp.Init();
                _preyAlive.Add(pp);
                pp.OnKilled += OnKilled;
            }
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