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
        
        public void StopMoving()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }


        private IEnumerator InputProcessing()
        {

            yield return null;
        }


        private IEnumerator AutoMoving()
        {
            yield return null;
        }
        
        private void PauseAutoMove()
        {
            
        }

        private void LaunchAutoMove()
        {
            
        }
        
        
    }
}