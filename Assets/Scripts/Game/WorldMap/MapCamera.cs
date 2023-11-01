using System.Collections;
using UnityEngine;

namespace Game.WorldMap
{
    public class MapCamera : MonoBehaviour
    {
        [SerializeField] private Transform _movable;
        private Coroutine _moving;
        private Vector3 _targetPosition;
        private Vector3 _lookAtPosition;
        
        
        public void SetFarPoint(WorldMapCameraPoint point)
        {
            if(point == null)
                Debug.Log("POINT IS NULLLLLLLL");
            SetPositionAndLook( point.PointFar.position, point.LookAt);
        }
        
        public void SetClosePoint(WorldMapCameraPoint point)
        {
            SetPositionAndLook( point.PointClose.position, point.LookAt);
        }

        public void SetPosRot(Vector3 pos, Quaternion rot)
        {
            _movable.SetPositionAndRotation(pos, rot);
        }
        
        public void MoveFarToClose(WorldMapCameraPoint point, float duration)
        {
            StopMoving();
            _moving = StartCoroutine(Moving(point, duration));
        }

        private void SetPositionAndLook(Vector3 pos, Transform lookAt)
        {
            var rotation = Quaternion.LookRotation(lookAt.position - pos);
            _movable.SetPositionAndRotation(pos, rotation);        
        }

        public void MoveBetweenPoints(WorldMapCameraPoint point1, WorldMapCameraPoint point2, float duraiton)
        {
            var rot1 = Quaternion.LookRotation(point1.LookAt.position - point1.PointClose.position);
            var rot2 = Quaternion.LookRotation(point2.LookAt.position - point2.PointClose.position);
            _moving = StartCoroutine(Moving(point1.PointClose.position, point2.PointClose.position,
                rot1, rot2, duraiton));
        }
        
        private IEnumerator Moving(WorldMapCameraPoint point, float duration)
        {
            var elapsed = 0f;
            var time = duration;

            while (elapsed <= time)
            {
                var pos = Vector3.Lerp(point.PointFar.position, point.PointClose.position, elapsed / time);
                SetPositionAndLook(pos, point.LookAt);
                elapsed += Time.deltaTime;
                yield return null;
            }
            SetPositionAndLook(point.PointClose.position, point.LookAt);
        }
        
        private IEnumerator Moving(Vector3 p1, Vector3 p2, Quaternion rot1, Quaternion rot2, float duration)
        {
            var elapsed = 0f;
            var time = duration;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                var rot = Quaternion.Lerp(rot1, rot2, t);
                var pos = Vector3.Lerp(p1, p2, t);
                _movable.SetPositionAndRotation(pos, rot);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _movable.SetPositionAndRotation(p2, rot2);
        }
        
        public void StopMoving()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }

        
        
        
    }
}