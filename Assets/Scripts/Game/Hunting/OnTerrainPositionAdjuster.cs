using System;
using UnityEngine;

namespace Game.Hunting
{
    [DefaultExecutionOrder(200)]
    public class OnTerrainPositionAdjuster : MonoBehaviour
    {
        [SerializeField] private LayerMask _mask;
        [SerializeField] private Transform _movable;
        
    
        private void Update()
        {
            var position = _movable.position;
            if (Physics.Raycast(position + Vector3.up * 50, Vector3.down, out var hit, 200f, _mask))
                position.y = hit.point.y;
            _movable.position = position;   
        }
    }
    
    
}