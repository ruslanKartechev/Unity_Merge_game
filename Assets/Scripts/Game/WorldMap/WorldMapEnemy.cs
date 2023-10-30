using System;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapEnemy : MonoBehaviour
    {
        [SerializeField] private string _animtionKey;
        [SerializeField] private Animator _animator;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            _animator.Play(_animtionKey);
        }

    }
}