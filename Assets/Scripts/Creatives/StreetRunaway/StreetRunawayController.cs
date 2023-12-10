using System.Collections;
using System.Collections.Generic;
using Creatives.Office;
using Dreamteck.Splines;
using UnityEngine;


namespace Creatives.StreetRunaway
{
    public class StreetRunawayController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _huntersGo;
        [SerializeField] private SplineFollower _hunterFollower;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _cameraDelay = 1f;
        
        private Coroutine _working;
        private List<ICreosHunter> _hunters;
        private int _index = 0;
        
        
        private void Start()
        {
            _hunters = new List<ICreosHunter>();
            foreach (var go in _huntersGo)
            {
                if (go == null)
                    continue;
                var ss = go.GetComponent<ICreosHunter>();
                if (ss != null)
                {
                    _hunters.Add(ss);
                    ss.OnDead += OnHunterDead;
                }
            }
            BeginMoving();
        }


        private void BeginMoving()
        {
            _hunterFollower.followSpeed = _moveSpeed;
            _hunterFollower.enabled = true;
            _hunterFollower.follow = true;
            if(_hunters.Count > 0)
                _hunters[0].SetActive();
        }

        private void ActivateCurrent()
        {
            if (_hunters.Count <= _index)
            {
                return;
            }
            var shark = _hunters[_index];
            shark.SetActive();
        }
        
        
        private void OnHunterDead(ICreosHunter shark)
        {
            _index++;
            ActivateCurrent();
            // StartCoroutine(DelayedCameraCall(_cameraDelay));
        }
        
        private IEnumerator DelayedCameraCall(float delay)
        {
            yield return new WaitForSeconds(delay);
            if(_index >= _hunters.Count)
                yield break;
        }
        
    }
}