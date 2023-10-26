using UnityEngine;

namespace Game.WorldMap
{
    public class MapCamera : MonoBehaviour
    {
        [SerializeField] private Transform _movable;
        private Coroutine _moving;
        
        public void SetPosition(WorldMapCameraPoint point)
        {
            _movable.position = point.Point.position + point.Offset;
        }

        
        public void StopMoving()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }
        
    }
}