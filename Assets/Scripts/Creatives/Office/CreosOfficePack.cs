using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

namespace Creatives.Office
{
    public class CreosOfficePack : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _animals;
        [SerializeField] private SplineFollower _follower;
        private List<ICreosHunter> _hunters;
        private int _index;
        
        private void Awake()
        {
            _hunters = new List<ICreosHunter>(_animals.Count);
            foreach (var aa in _animals)
            {
                if(aa == null)
                    continue;
                var hunter = aa.GetComponent<ICreosHunter>();
                if (hunter != null)
                {
                    _hunters.Add(hunter);
                    hunter.Run();
                    hunter.OnDead += OnDead;
                }
            }
        }
        
        private void OnDead(ICreosHunter hunter)
        {
            hunter.OnDead -= OnDead;
            _index++;
            if (_index >= _hunters.Count)
                return;
            ActivateCurrentHunter();
        }

        private void Start()
        {
            ActivateCurrentHunter();
            StartMoving();
        }
        
        private void StartMoving()
        {
            _follower.enabled = true;
            _follower.follow = true;
        }

        private void ActivateCurrentHunter()
        {
            _hunters[_index].SetActive();   
        }        
    }
}