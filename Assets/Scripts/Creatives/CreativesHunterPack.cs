using System;
using System.Collections.Generic;
using Common;
using Common.Utils;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.Hunters;
using Game.Hunting.Hunters.Interfaces;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Creatives
{
    public class CreativesHunterPack : MonoBehaviour
    {
        public event Action OnAllWasted;

        [SerializeField] private HunterAimer _hunterAimer;
        [SerializeField] private List<GameObject> _huntersGos;
        private ICamFollower _camFollower;
        private IPreyPack _preyPack;
        private IList<IHunter> _hunters;
        private IList<IHunter> _activeHunters;
        private int _currentHunterIndex;
        private HuntersBush _bush;
        private bool _beganRunning;
        private HunterTargetPicker _targetPicker;
        
        private MovementTracks _tracks;
        private IHunter currentHunter => _activeHunters[_currentHunterIndex];
        
        
        public void Init(IPreyPack preyPack, ProperButton inputButton, GameObject camera, MovementTracks track)
        {
            _tracks = track;
            _preyPack = preyPack;
            _camFollower = camera.GetComponent<ICamFollower>();
            var jumpCam = camera.GetComponent<IJumpCamera>();
            _activeHunters = new List<IHunter>();
            _hunters = new List<IHunter>();
            foreach (var go in _huntersGos)
            {
                var hunter = go.GetComponent<IHunter>();
                _hunters.Add(hunter);
                _activeHunters.Add(hunter);
                hunter.CamFollower = jumpCam;
                hunter.OnDead += OnHunterDead;
                hunter.InitSelf(track);
            }
            _hunterAimer.InputButton = inputButton;
            _targetPicker = new HunterTargetPicker(_preyPack);
        }
       
        
        public void IdleState()
        {
            CLog.LogWHeader(nameof(HunterPack), "Idle state", "g", "w");
            foreach (var hunter in _activeHunters)
                hunter.Idle();
            var firstHunter = _hunters[0];
            var preyTarget = _targetPicker.GetBestPrey(firstHunter);
            firstHunter.RotateTo(((MonoBehaviour)preyTarget).transform.position);
            _currentHunterIndex = 0;
            // var tr = firstHunter.GetTransform();
            // _bush = _hunterBushSpawner.SpawnBush(tr.position, tr.rotation);
        }

        public void AllowAttack()
        {
            _hunterAimer.SetHunter(currentHunter);
            _hunterAimer.Activate();
        }

        public void Win()
        {
            foreach (var hunter in _activeHunters)
                hunter.OnDead -= OnHunterDead;
            foreach (var hunter in _activeHunters)
                hunter.Celebrate();
            _hunterAimer.Stop();
        }

        public float TotalPower()
        {
            var power = 0f;
            foreach (var hunter in _activeHunters)
                power += hunter.Settings.Damage;
            return power;
        }

        public void FocusCamera(bool animated = true)
        {
            var fistHunter = _hunters[0];
            _targetPicker.PickHunterCamTarget(fistHunter, out var target);
            _camFollower.FollowAndLook(currentHunter.CameraPoint,
                target, 
                !animated);
        }
        
                
        public void BeginChase()
        {
            CLog.LogWHeader(nameof(HunterPack), "Run State", "g", "w");
            _beganRunning = true;
            foreach (var hunter in _activeHunters)
                hunter.Run();
            _bush?.Hide();
        }

        private void NextIndex()
        {
            _currentHunterIndex++;
            if (_currentHunterIndex >= _activeHunters.Count)
                _currentHunterIndex = 0;
        }

        private void NextHunter(bool indexUp)
        {
            // CLog.LogWHeader("HunterPack", "Next Hunter", "w");
            if(indexUp)
                NextIndex();
            SetActiveHunter();
        }

        private void SetActiveHunter()
        {
            var currentHunter = _activeHunters[_currentHunterIndex];
            if (_targetPicker.PickHunterCamTarget(currentHunter, out var lookTarget))
            {
                _camFollower.FollowAndLook(currentHunter.CameraPoint, lookTarget);
            }
            else
            {
                // all targets are dead, look in the dir of the hunter
                _camFollower.FollowOne(currentHunter.CameraPoint);
            }
            _hunterAimer.SetHunter(currentHunter);
        }
        
        private void OnHunterDead(IHunter hunter)
        {
            if (_activeHunters.Count == 1)
            {
                CLog.LogWHeader("HunterPack", "All hunters dead", "w");
                _activeHunters.Remove(hunter);
                SetCameraToPrey();
                OnAllWasted?.Invoke();
                return;
            }
            _activeHunters.Remove(hunter);
            NextHunter(false);
            _hunterAimer.Activate();
        }

        private void SetCameraToPrey()
        {
            _camFollower.FollowFromBehind(_preyPack.CamTarget);
        }

        
        #if UNITY_EDITOR
        [ContextMenu("Get Hunters")]
        public void GetHunters()
        {
            var parent = transform;
            var hunters = HierarchyUtils.GetFromAllChildren<OnTerrainPositionAdjuster>(parent, (t) =>
            {
                return t.GetComponent<IHunter>() != null;
            });
            foreach (var item in hunters)
            {
                if(_huntersGos.Contains(item.gameObject) == false)
                    _huntersGos.Add(item.gameObject);
            }

            for (var i = _huntersGos.Count - 1; i >= 0; i--)
            {
                if(_huntersGos[i] == null)
                    _huntersGos.RemoveAt(i);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }
}