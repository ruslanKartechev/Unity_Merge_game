using System.Collections.Generic;
using UnityEngine;

namespace Creatives.Firemen
{
    public class KongEnemiesPushBack : JumpDownKongListener
    {
        [SerializeField] private List<GameObject> _targets;


        public override void OnLanded(Vector3 position)
        {
            foreach (var target in _targets)
            {
                if(target == null)
                    continue;
                if (target.TryGetComponent<IPushBackTarget>(out var tt))
                {
                    tt.PushBack(position);
                }
            }
        }
    }
}