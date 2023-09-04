using System;
using System.Collections;
using Common;
using Common.Ragdoll;
using Game.Hunting.HuntCamera;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public class SimpleHunter : MonoBehaviour, IHunter
    {
        private static readonly int RunKey = Animator.StringToHash("Move");
        private static readonly int JumpKey = Animator.StringToHash("Win");
        private const float AfterAttackDelay = 1f;
        public event Action<IHunter> OnDead;
        
        [SerializeField] private CamFollowTarget _camFollowTarget;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _movable;
        [SerializeField] private IRagdoll _ragdoll;
        [SerializeField] private HunterMouth _mouthPrefab;
        [SerializeField] private Transform _raycastDir;
        [SerializeField] private Rigidbody _headRb;
        [SerializeField] private float _sphereCastRad = 0.3f;
        private IHunterSettings _settings;
        private IPrey _prey;
        private Coroutine _moving;

        public void Init(IHunterSettings settings)
        {
            _settings = settings;
        }

        public void SetPrey(IPrey prey)
        {
            _prey = prey;
        }
        

        public void Run()
        {
            _animator.SetTrigger(RunKey);
        }

        public ICamFollowTarget GetCameraPoint() => _camFollowTarget;
        public Transform GetTransform() => transform;

        public void Jump(AimPath path)
        {
            _animator.SetTrigger(JumpKey);
            _movable.SetParent(null);
            
            if(_moving != null)
                StopCoroutine(_moving);
            _moving = StartCoroutine(Jumping(path));
        }

        public void Celebrate()
        {
            _animator.SetTrigger("Idle");
        }

        private IEnumerator Jumping(AimPath path)
        {
            var time = ((path.end - path.inflection).magnitude + (path.inflection - path.start).magnitude) 
                            / _settings.JumpSpeed;
            var elapsed = 0f;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                var pos = Bezier.GetPosition(path.start, path.inflection, path.end, t);
                var endRot = Quaternion.LookRotation(path.end - _movable.position);
                _movable.rotation = Quaternion.Lerp(_movable.rotation, endRot, 0.1f);
                _movable.position = pos;    
                elapsed += Time.deltaTime;
                if (TryBite())
                {
                    yield return new WaitForSeconds(AfterAttackDelay);   
                    OnDead?.Invoke(this);
                    yield break;
                }
                yield return null;
            }
            _animator.enabled = false;
            _ragdoll.Activate();
            yield return new WaitForSeconds(AfterAttackDelay);
            OnDead?.Invoke(this);
        }

        private bool TryBite()
        {
            // var start = _raycastDir.position;
            // var end = _raycastDir.position + _raycastDir.forward * _settings.BiteDistance;
            // Debug.DrawLine(start, end, Color.blue, 0.5f);
            if (Physics.SphereCast(_raycastDir.position, _sphereCastRad, _raycastDir.forward, 
                    out var hit, _settings.BiteDistance, _settings.BiteMask))
            {
                var target = hit.collider.gameObject.GetComponent<IBiteTarget>();
                target.Damage(new DamageArgs(_settings.Damage, hit.point));
                _animator.enabled = false;
                _ragdoll.Activate();
                var mouth = Instantiate(_mouthPrefab);
                mouth.transform.position = hit.point - (hit.point - _raycastDir.position).normalized * _settings.BiteOffset;
                mouth.BiteTo(target.GetBiteBone(), _headRb);   
                
                return true;
            }
            return false;
        }
    }
}