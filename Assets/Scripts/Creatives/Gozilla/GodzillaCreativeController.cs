using System.Collections;
using System.Collections.Generic;
using Creatives.Office;
using Dreamteck.Splines;
using UnityEngine;

namespace Creatives.Gozilla
{
    public class GodzillaCreativeController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _hunters;
        [SerializeField] private GodzillaController _godzilla;
        [SerializeField] private SplineFollower _hunterFollower;
        [SerializeField] private SplineFollower _godzillaFollower;
        [SerializeField] private float _moveSpeed;
        private Coroutine _working;
        private List<ICreosHunter> _sharks;
        private int _index = 0;
        
        private void Start()
        {
            _sharks = new List<ICreosHunter>();
            foreach (var go in _hunters)
            {
                if (go == null)
                    continue;
                var ss = go.GetComponent<ICreosHunter>();
                if (ss != null)
                {
                    _sharks.Add(ss);
                    ss.OnDead += OnHunterDead;
                }
            }
            StartCoroutine(Working());
        }


        private void BeginMoving()
        {
            _hunterFollower.followSpeed = _moveSpeed;
            _godzillaFollower.followSpeed = _moveSpeed;
            _hunterFollower.enabled = _godzilla.enabled = true;
            _hunterFollower.follow = _godzillaFollower.follow = true;
            if(_sharks.Count > 0)
                _sharks[0].SetActive();
        }

        private void ActivateCurrent()
        {
            if (_sharks.Count <= _index)
            {
                return;
            }
            var shark = _sharks[_index];
            shark.SetActive();
        }
        
        
        private void OnHunterDead(ICreosHunter shark)
        {
            _index++;
            ActivateCurrent();
        }
        
        private IEnumerator Working()
        {
            BeginMoving();
            yield return null;
        }
        
    }
}