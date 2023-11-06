using System.Collections;
using Game.Hunting;
using Game.Hunting.Hunters.Interfaces;
using UnityEngine;

namespace Game.Merging
{
    public class MergeItemView : MonoBehaviour, IMergeItemView
    {
        [SerializeField] private float _snapTime;
        [SerializeField] private Transform _movable;
        [SerializeField] private Vector3 _spawnedPositionOffset;
        [SerializeField] private Transform _modelPoint;
        [SerializeField] private ItemDamageDisplay _itemDamageDisplay;
        private Coroutine _snapping;
        private IMergeItemHighlighter _highlighter;

        
        private void Awake()
        {
            _highlighter = GetComponent<IMergeItemHighlighter>();
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

        
        public void JumpForward(Vector2 dirs, float delay, float time)
        {
            var animator = gameObject.GetComponentInChildren<HunterAnimator>();
            var animReceiver = gameObject.GetComponentInChildren<HunterAnimEventReceiver>();
            // if (animator == null)
            // {
            //     Debug.Log($"no animator {gameObject.name}");
            //     return;
            // }
            if (animReceiver == null)
            {
                StartCoroutine(Jumping(dirs, delay, time));
                return;
            }
            animReceiver.OnJumpEvent += () =>
            {
                StartCoroutine(Jumping(dirs, delay, time));
            };
            animator.Jump();
        }

        private IEnumerator Jumping(Vector2 dirs, float delay, float time)
        {
            yield return new WaitForSeconds(delay);
            var tr = transform;
            var start = tr.position;
            var end = start + tr.forward * dirs.x;
            var inf = Vector3.Lerp(start, end, .4f) + Vector3.up * dirs.y;
            var elapsed = 0f;
            var t = elapsed / time;
            while (t <= .6)
            {
                tr.position = Common.Bezier.GetPosition(start, inf, end, t);
                t = elapsed / time;
                elapsed += Time.deltaTime;
                yield return null;
            }
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