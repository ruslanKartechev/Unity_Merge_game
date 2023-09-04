using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    [DefaultExecutionOrder(101)]
    public class HunterPackMover : MonoBehaviour
    {
        [SerializeField] private Transform _movable;
        private IPrey _prey;
        private IList<IHunter> _hunters;
        private Coroutine _moving;
        
        
        public void Init(IPrey prey, IList<IHunter> hunters)
        {
            _prey = prey;
            _hunters = hunters;
        }
        
        public void StartMoving()
        {
            StopMovement();
            _moving = StartCoroutine(Moving());
        }

        public void StopMovement()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }

        private IEnumerator Moving()
        {
            var targetPos = _prey.GetPosition();
            var offset = (_movable.position - targetPos);
            offset.y = 0;
            while (true)
            {
                targetPos = _prey.GetPosition();
                targetPos.y = 0;
                _movable.rotation = _prey.GetRotation();
                _movable.position = targetPos + offset;
                yield return null;
            }
        }
        

    }
}