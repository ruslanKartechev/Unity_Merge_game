using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    [DefaultExecutionOrder(101)]
    public class HunterPackMover : MonoBehaviour
    {
        [SerializeField] private Transform _movable;
        private IPreyPack _prey;
        private Coroutine _moving;
        private IList<IHunter> _hunters;
        
        
        public void Init(IPreyPack prey, IList<IHunter> hunters)
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
            var targetPos = _prey.Position;
            var offset = (_movable.position - targetPos);
            offset.y = 0;
            while (true)
            {
                targetPos = _prey.Position;
                targetPos.y = 0;
                _movable.rotation = _prey.Rotation;
                _movable.position = targetPos + offset;
                yield return null;
            }
        }
        

    }
}