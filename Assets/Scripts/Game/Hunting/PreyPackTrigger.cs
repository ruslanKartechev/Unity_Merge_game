﻿using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyPackTrigger : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _listeners;
        
        public void Activate(bool active)
        {
            gameObject.SetActive(active);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<IHunter>() != null)
            {
                Debug.Log("HUNTER TRIGGER ENTER!!!!!!!");
                foreach (var go in _listeners)
                    go.GetComponent<IPreyTriggerListener>().OnAttacked();
                Activate(false);
            }
        }
    }
}