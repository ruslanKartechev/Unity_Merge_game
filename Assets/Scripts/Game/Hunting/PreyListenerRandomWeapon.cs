using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyListenerRandomWeapon : PreyActionListener
    {
        [SerializeField] private List<ColdWeapon> _weapons;
        private ColdWeapon _active;

        public override void OnInit()
        {
            _active = _weapons.Random();
            _active.Show();
            foreach (var weapon in _weapons)
            {
                if(weapon != _active)
                    weapon.Hide();
            }
        }

        public override void OnDead()
        {
            _active.Drop();   
        }

        public override void OnBeganRun()
        { }
    }
}