using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyRandomWeaponPicker : MonoBehaviour
    {
        [SerializeField] private List<ColdWeapon> _weapons;
        private ColdWeapon _active;

        public void SetRandomWeapon()
        {
            _active = _weapons.Random();
            if(_active != null)
                _active.Show();
            foreach (var weapon in _weapons)
            {
                if(weapon != _active)
                    weapon.Hide();
            }
        }

        public void HideAll()
        {
            foreach (var weapon in _weapons)
            {
                weapon.gameObject.SetActive(false);
            }
        }

        public void DropWeapon()
        {
            _active?.Drop();   
        }
        
    }
}