using System;
using System.Collections;
using System.Collections.Generic;
using Game.Hunting.HuntCamera;
using UnityEngine;
using Utils;

namespace Game.Hunting
{
    public class PreyPack : MonoBehaviour, IPreyPack
    {
        public event Action OnAllDead;
        public event Action<IPrey> OnPreyKilled;
        public event Action OnBeganMoving;

        [SerializeField] private float _surprisedTime = 1f;
        [Space(10)]
        [SerializeField] private Transform _movable;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private PreyPackCameraTrajectory _preyPackCamera;
        [SerializeField] private List<MonoBehaviour> _prey;

        private IPreyPackMover _mover;
        private HashSet<IPrey> _preyAlive;

        public HashSet<IPrey> GetPrey()
        {
            return _preyAlive;
        }

        public void Init(MovementTracks track, ILevelSettings levelSettings)
        {
            _mover = _movable.GetComponent<IPreyPackMover>();
            _mover.Init(track);
            
            _preyAlive = new HashSet<IPrey>(_prey.Count);
            var settings = levelSettings.PreySettingsList;
            for (var i = 0; i < _prey.Count; i++)
            {
                var pp = _prey[i] as IPrey;
                pp.PreySettings = settings[i]; 
                pp.Init();
                _preyAlive.Add(pp);
                pp.OnKilled += OnKilled; 
                
            }
        }

        public Vector3 Position => _movable.position;
        
        public Quaternion Rotation => _movable.rotation;
        
        public Vector3 LocalToWorld(Vector3 position) => _movable.TransformPoint(position);
        
        public ICamFollowTarget CamTarget => _camFollowTarget;        

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

        public float TotalPower()
        {
            var power = 0f;
            foreach (var prey in _preyAlive)
                power += prey.PreySettings.Health;
            return power;
        }

        public void Idle()
        {
            CLog.LogWHeader(nameof(PreyPack), "Prey pack IDLE", "b","w");
            _mover.StopMoving();
            foreach (var prey in _preyAlive)
                prey.IdleState();
        }
        
        public void RunAttacked()
        {
            CLog.LogWHeader(nameof(PreyPack), "ON Attacked", "g");
            foreach (var prey in _preyAlive)
                prey.SurpriseToAttack();
            StartCoroutine(DelayedRun());
        }
        
        
        private IEnumerator DelayedRun()
        {
            yield return new WaitForSeconds(_surprisedTime);
            if (_preyAlive.Count == 0)
                yield break;
            _mover.BeginMoving();
            foreach (var prey in _preyAlive)
                prey.RunState();
            OnBeganMoving.Invoke();
        }
        
        private void OnKilled(IPrey prey)
        {
            _preyAlive.Remove(prey);
            OnPreyKilled?.Invoke(prey);
            if (_preyAlive.Count == 0)
            {
                CLog.LogWHeader("PreyPack", "On prey killed", "g", "w");
                _mover.StopMoving();
                OnAllDead?.Invoke();
            }
        }
    }
}