using System;
using Dreamteck.Splines;
using UnityEngine;

namespace Creatives.Kong
{
    public class KongController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _runAnim;
        [SerializeField] private SplineFollower _follower;
        [SerializeField] private KongTrigger _kongTrigger;
        
        private void Start()
        {
            _animator.Play(_runAnim);
            _follower.enabled = true;
            _follower.follow = true;
        }
    }
}