using System;
using UnityEngine;

namespace Game.Hunting
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] private GameObject _go;

        public void Drop()
        {
            _go.SetActive(false);
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_go == null)
                _go = gameObject;
        }
        #endif
    }
}