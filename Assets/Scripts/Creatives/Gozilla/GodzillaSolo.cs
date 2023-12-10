using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Creatives.Firemen;
using Creatives.Kong;
using Creatives.Office;
using Dreamteck.Splines;


namespace Creatives.Gozilla
{
    public class GodzillaSolo : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private SplineFollower _godzillaFollower;
        [SerializeField] private SplineFollower _shipFollower;
        [Space(10)]
        [SerializeField] private Transform _flyToTarget;
        [SerializeField] private Transform _movable;
        [SerializeField] private CreosAimer _aimer;
        [SerializeField] private Animator _animator;
        [SerializeField] private CreosSettings _creosSettings;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private List<GameObject> _pushTargets;
        private Coroutine _input;
        private Coroutine _jumping;

        private void Start()
        {
            _godzillaFollower.followSpeed = _shipFollower.followSpeed = _moveSpeed;
            _godzillaFollower.follow = _shipFollower.follow = true;
            StartInput();
            _aimer.Activate();
        }
        
        public void Attack()
        {
            StopJump();
            _jumping = StartCoroutine(JumpingCustomTarget(_flyToTarget));
        }

        private void StopJump()
        {
            if(_jumping != null)
                StopCoroutine(_jumping);
        }
        
        private void StartInput()
        {
            StopInput();
            _input = StartCoroutine(Inputting());
        }
        
        private void StopInput()
        {
            if(_input != null)
                StopCoroutine(_input);
        }
        

        public void PlayJumpAnim()
        {
            _animator.SetTrigger(_creosSettings.attackKey);
        }
        
        private IEnumerator JumpingCustomTarget(Transform target)
        {
            _godzillaFollower.follow = false;
            transform.parent = null;
            var path = _aimer.Path;
            var elapsed = Time.deltaTime;
            var t = 0f;
            var time = ((target.position - path.inflection) + (path.inflection - path.start)).magnitude / _creosSettings.jumpSpeed;
            var curve = _creosSettings.jumpCurve;
            while (t <= 1)
            {
                var pos = Bezier.GetPosition(path.start, path.inflection, target.position, t);
                var rotVec = target.position - _movable.position;
                rotVec.y = 0f;
                var rot2 = Quaternion.LookRotation(rotVec);
                _movable.position = pos;
                t = elapsed / time;
                elapsed += Time.deltaTime * curve.Evaluate(t);
                yield return null;
            }
            _movable.position = target.position;
            _movable.parent = target;
            PlayParticles();
            foreach (var go in _pushTargets)
            {
                if(go == null)
                    continue;
                var tr = go.GetComponent<IPushBackTarget>();
                if(tr != null)
                    tr.PushBack(transform.position);
            }
        }

        private void PlayParticles()
        {
            if (_particles != null)
            {
                _particles.gameObject.SetActive(true);
                _particles.Play();
            }
        }

        private IEnumerator Inputting()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // Debug.Log("Start Aim");
                    _aimer.OnDown();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    // Debug.Log("Release");
                    _aimer.OnUp();
                    PlayJumpAnim();
                }
                yield return null;
            }
        }

     
    }
}