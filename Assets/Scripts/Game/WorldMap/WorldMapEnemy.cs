using System;
using Game.Hunting;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapEnemy : MonoBehaviour
    {
        [SerializeField] private string _animtionKey;
        [SerializeField] private Animator _animator;
        [Space(10)]
        [SerializeField] private bool _hasWeapon = true;
        [SerializeField] private bool _randomWeapon = true;
        [SerializeField] private PreyRandomWeaponPicker _weaponPicker;
        private bool _inited;

        
        private void Start()
        {
            Init();
        }

        public void Init()
        {
            if(_inited)
                return;
            _inited = true;
            _animator.Play(_animtionKey);
            if (!_hasWeapon)
            {
                _weaponPicker.HideAll();
                return;
            }
            if(_randomWeapon)
                _weaponPicker.SetRandomWeapon();
        }
        

    }
}