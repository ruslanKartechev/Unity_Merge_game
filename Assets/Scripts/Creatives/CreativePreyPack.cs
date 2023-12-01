using System;
using System.Collections.Generic;
using Common.Utils;
using Common.Utils.EditorUtils;
using Game.Hunting;
using Game.Hunting.HuntCamera;
using Game.Hunting.Prey;
using Game.Hunting.Prey.Interfaces;
using Game.Levels;
using UnityEditor;
using UnityEngine;

namespace Creatives
{
    public class CreativePreyPack : MonoBehaviour, IPreyPack
    {
        public event Action OnAllDead;
        public event Action<IPrey> OnPreyKilled;
        public event Action OnBeganMoving;
        
        [SerializeField] private List<GameObject> _enemies;
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private PreyPackMover _preyPackMover;
        
        public int PreyCount { get; }
        public ICamFollowTarget CamTarget => _camFollowTarget;
        
        private HashSet<IPrey> _prey;
        public HashSet<IPrey> GetPrey()
        {
            return _prey;
        }

        public void Init(MovementTracks track, ILevelSettings levelSettings)
        {
            _preyPackMover.Init(track);
            _prey = new HashSet<IPrey>();
            foreach (var enemy in _enemies)
            {
                if (enemy.TryGetComponent<CreativeCar>( out var car))
                {
                    _prey.Add(car);
                    car.Init();         
                }
                else if(enemy.TryGetComponent<CreativeBoat>( out var boat))
                {
                    _prey.Add(boat);
                    boat.Init();
                }        
                else if(enemy.TryGetComponent<CreativeAnimal>( out var animal))
                {
                    _prey.Add(animal);
                    animal.Init();
                }    
                else if(enemy.TryGetComponent<PreyBarbarian>( out var barb))
                {
                    _prey.Add(barb);
                    barb.Init();
                }    

            }
        }

        public void Idle()
        { }

        public void RunAttacked()
        {
            _preyPackMover.BeginMoving();
            foreach (var prey in _prey)
                prey.OnPackRun();
        }

        public void RunCameraAround(GameObject cam, Action returnCamera)
        {
            returnCamera?.Invoke();
        }

        public float TotalPower()
        {
            return 100;
        }
        
        #if UNITY_EDITOR
        [ContextMenu("Get Enemies")]
        public void GetEnemies()
        {
            for (var i = _enemies.Count - 1; i >= 0 ; i--)
            {
                if(_enemies[i] == null)
                    _enemies.RemoveAt(i);
            }
            var results = HierarchyUtils.GetFromAllChildren<Transform>(transform, (t) =>
            {
                return t.TryGetComponent<IPrey>(out var prey);
            });
            foreach (var prey in results)
            {
                if(_enemies.Contains(prey.gameObject) == false)
                    _enemies.Add(prey.gameObject);
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
    

    
#if UNITY_EDITOR
    [CustomEditor(typeof(CreativePreyPack))]
    public class CreativePreyPackEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as CreativePreyPack;

            if (EU.ButtonBig("Get", Color.blue))
            {
                me.GetEnemies();
            }
        }
    }
    #endif
    
}