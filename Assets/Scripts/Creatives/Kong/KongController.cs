using System.Collections;
using Dreamteck.Splines;
using Game.Hunting;
using Game.Hunting.Hunters;
using UnityEngine;

namespace Creatives.Kong
{
    public class KongController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _runAnim;
        [SerializeField] private SplineFollower _follower;
        [SerializeField] private CreosAimer _aimer;
        [SerializeField] private KongJumper _kongJumper;
        [SerializeField] private KongTrigger _kongTrigger;
        [SerializeField] private AimVisualizer _aimVisualizer;
        [SerializeField] private bool _run;
        private HunterAimer Aimer;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            _animator.Play(_runAnim);
            if(_run)
            {
                _follower.enabled = true;
                _follower.follow = true;         
            }
            _aimer.Activate();
            StartCoroutine(Working());
        }

        private void JumpOnPath()
        {
            Debug.Log("Jump");
            if (_follower != null)
                _follower.follow = false;
            var path = _aimer.Path;
            _kongJumper.Jump(path);
        }
        
        private IEnumerator Working()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Start Aim");
                    _aimer.OnDown();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("Release");
                    _aimer.OnUp();
                    JumpOnPath();
                }
                yield return null;
            }
        }
        
        
    }
}