using System.Collections.Generic;
using Common;
using Game.Hunting;
using Game.Hunting.Prey;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapEnemy : MonoBehaviour
    {
        [SerializeField] private string _animtionKey;
        [SerializeField] private List<string> _deadAnimations;
        [SerializeField] private Animator _animator;
        [Space(10)]
        [SerializeField] private bool _hasWeapon = true;
        [SerializeField] private bool _randomWeapon = true;
        [SerializeField] private PreyRandomWeaponPicker _weaponPicker;
        private bool _inited;
        

        
        // private void Start()
        // {
        //     Init();
        // }

        public void Active()
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

        public void Dead()
        {
            if(_inited)
                return;
            _inited = true;
            
            _animator.Play(_deadAnimations.Random());
            if (!_hasWeapon)
            {
                _weaponPicker.HideAll();
                return;
            }
            if (_randomWeapon)
                _weaponPicker.SetRandomWeapon();
            _weaponPicker.DropWeapon();
        }
    }
}