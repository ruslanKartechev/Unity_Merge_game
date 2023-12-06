using System.Collections.Generic;
using UnityEngine;

namespace Creatives.Firemen
{
    public class KongEnemiesPushBack : JumpDownKongListener
    {
        [SerializeField] private List<GameObject> _targets;

        public void GetRefs()
        {
#if UNITY_EDITOR
            var firemen = FindObjectsOfType<FiremanTarget>();
            foreach (var ff in firemen)
            {
                if(_targets.Contains(ff.gameObject) == false)
                    _targets.Add(ff.gameObject);
            }

            for (var i = _targets.Count - 1; i >= 0; i--)
            {
                if (_targets[i] == null)
                    _targets.RemoveAt(i);
            }
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

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