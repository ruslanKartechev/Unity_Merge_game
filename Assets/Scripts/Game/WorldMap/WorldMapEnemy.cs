using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapEnemy : MonoBehaviour
    {
        [SerializeField] private string _animtionKey;
        [SerializeField] private Animator _animator;

        public void Init()
        {
            _animator.Play(_animtionKey);
        }

    }
}