using System.Collections;
using Game.Hunting.Hunters;
using Game.Hunting.Hunters.Interfaces;
using Game.Merging.Interfaces;
using UnityEngine;

namespace Game.Merging.View
{
    public class MergeItemView : MonoBehaviour, IMergeItemView
    {
        [SerializeField] private float _snapTime;
        [SerializeField] private Transform _movable;
        [SerializeField] private Vector3 _spawnedPositionOffset;
        [SerializeField] private Transform _modelPoint;
        [SerializeField] private ItemDamageDisplay _itemDamageDisplay;
        [SerializeField] private HunterAnimator _animator;
        [SerializeField] private HunterAnimEventReceiver _animEventReceiver;
        private Coroutine _snapping;
        private IMergeItemHighlighter _highlighter;
        private const float RotationTime = .5f;

        
        private void Awake()
        {
            _highlighter = GetComponent<IMergeItemHighlighter>();
            _animator = gameObject.GetComponentInChildren<HunterAnimator>();
            _animEventReceiver = gameObject.GetComponentInChildren<HunterAnimEventReceiver>();
        }

        public Quaternion Rotation
        {
            get => _movable.rotation;
            set => _movable.rotation = value;
        }
 
        private MergeItem _item;
        public MergeItem Item
        {
            get => _item;
            set => _item = value;
        }

        public void DisplaySetActive(bool active)
        {
            _itemDamageDisplay.gameObject.SetActive(active);
        }
        
        public void SetSettings(object settings)
        {
            _itemDamageDisplay.SetDamage(((IHunterSettings)settings).Damage);
        }

        public void OnSpawn()
        {
            var pos = _movable.position;
            CorrectedPosition(ref pos);
            _movable.position = pos + Vector3.up;
            StopSnapping();
            _snapping = StartCoroutine(Snapping(pos));
        }

        public void SetPositionRotation(Vector3 position, Quaternion rotation)
        {
            _movable.position = position;
            _movable.rotation = rotation;
        }

        public void SetDraggedPosition(Vector3 position)
        {
            _movable.position = position + _spawnedPositionOffset;
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public void SnapToPos(Vector3 position)
        {
            CorrectedPosition(ref position);
            // _movable.position = position + Vector3.up * OnSpawnUpOffset;
            StopSnapping();
            _snapping = StartCoroutine(Snapping(position));
        }

        public void OnPicked()
        {
            _highlighter.Highlight();
        }

        public void OnReleased()
        {
            _highlighter.Normal();
        }

        public Vector3 GetModelPosition() => _modelPoint.position;


        private const float JumpElevation = 5f;
        public void JumpToPoint(Vector3 endPoint, float delay, float time, bool rotate = true, float maxT = 1f)
        {
            StopAllCoroutines();
            _animator.Jump();
            if (_animEventReceiver == null)
            {
                StartCoroutine(Jumping(endPoint, delay, time, maxT,rotate));
                return;
            }
            _animEventReceiver.Clear();
            _animEventReceiver.OnJumpEvent += () =>
            {
                StartCoroutine(Jumping(endPoint, delay, time, maxT, rotate));
            };
        }

        public void PrepareToJump()
        {
            _animEventReceiver?.Clear();
            _animator.Prepare();
        }
        
        public void JumpForward(Vector2 dirs, float delay, float time, float maxT = 1f)
        {
            var endPoint = transform.position + transform.forward * dirs.x;
            StopAllCoroutines();
            _animator.Jump();
            if (_animEventReceiver == null)
            {
                StartCoroutine(Jumping(endPoint, delay, time, maxT, false));
                return;
            }
            _animEventReceiver.Clear();
            _animEventReceiver.OnJumpEvent += () =>
            {
                StartCoroutine(Jumping(endPoint, delay, time, maxT, false));
            };
        }

        private IEnumerator Jumping(Vector3 endPos, float delay, float time, float maxT, bool rotate)
        {
            yield return new WaitForSeconds(delay);
            var tr = transform;
            var start = tr.position;
            var end = endPos;
            var inf = Vector3.Lerp(start, end, .4f) + Vector3.up * JumpElevation;
            var elapsed = 0f;
            var t = elapsed / time;
            var landed = false;
            while (t <= maxT)
            {
                tr.position = Common.Bezier.GetPosition(start, inf, end, t);
                t = elapsed / time;
                elapsed += Time.deltaTime;
                if (!landed && maxT >= 1f && t >= .8f)
                {
                    landed = true;
                    _animator.Land();
                }
                yield return null;
            }

            if (rotate)
                yield return RotatingToForward();
        }

        private IEnumerator RotatingToForward()
        {
            var elapsed = 0f;
            var time = RotationTime;
            var startRot = _movable.localRotation;
            var endRot = Quaternion.identity;
            while (elapsed <= time)
            {
                _movable.localRotation = Quaternion.Lerp(startRot, endRot, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _movable.localRotation = endRot;
        }
        
        private void CorrectedPosition(ref Vector3 position)
        {
            position += _spawnedPositionOffset;
        }

        private void StopSnapping()
        {
            if(_snapping != null)
                StopCoroutine(_snapping);   
        }

        private IEnumerator Snapping(Vector3 endPos)
        {
            var elapsed = 0f;
            var startPos = _movable.position;
            while (elapsed < _snapTime)
            {
                _movable.position = Vector3.Lerp(startPos, endPos, elapsed / _snapTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _movable.position = endPos;
        }
    }
}