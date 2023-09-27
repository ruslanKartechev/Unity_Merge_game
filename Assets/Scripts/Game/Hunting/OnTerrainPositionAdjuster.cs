using UnityEngine;

namespace Game.Hunting
{
    [DefaultExecutionOrder(200)]
    public class OnTerrainPositionAdjuster : MonoBehaviour
    {
        [SerializeField] private LayerMask _mask;
        [SerializeField] private Transform _movable;
        [SerializeField] private float _upOffset = 0;

        public float Offset => _upOffset;
        
        private void Update()
        {
            var position = _movable.position;
            if (Physics.Raycast(position + Vector3.up * 50, Vector3.down, out var hit, 200f, _mask))
                position.y = hit.point.y + _upOffset;
            _movable.position = position;   
        }

        public Vector3 GetAdjustedPosition(Vector3 position)
        {
            if (Physics.Raycast(position + Vector3.up * 50, Vector3.down, out var hit, 200f, _mask))
                position.y = hit.point.y + _upOffset;
            return position;      
        }
    }
    
}