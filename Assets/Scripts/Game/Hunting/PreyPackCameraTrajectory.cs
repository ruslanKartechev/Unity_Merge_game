using System;
using System.Collections;
using System.Xml.Serialization;
using Common;
using Game.Hunting.HuntCamera;
using UnityEngine;
using Utils;

namespace Game.Hunting
{
    public class PreyPackCameraTrajectory : MonoBehaviour
    {
        [SerializeField] private Transform _lookAt;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _p1;
        [SerializeField] private Transform _p2;
        [SerializeField] private Transform _p3;
        [Space(10)] 
        [SerializeField] private bool _moveFrontAndBack;
        [Space(5)]
        [SerializeField] private float _moveStartDelay = 1f;
        [SerializeField] private float _endCallDelay = 1f;
        [SerializeField] private float _startToP1Speed = 10f;
        [SerializeField] private float _curveSpeed = 10f;
        [Space(5)]
        [SerializeField] private float _curveLength = 1;
        private Coroutine _moving;
        private const int CurvePointsCount = 30;
        private Action _onDone;

#if UNITY_EDITOR
        #region Gizmos
        public bool drawGizmos = true;
        public Color gizmosColor;
        
        private void OnDrawGizmos()
        {
            if (drawGizmos == false)
                return;
            if (_lookAt == null || _startPoint == null || _p1 == null || _p2 == null || _p3 == null)
                return;
            var oldColor = Gizmos.color;
            Gizmos.color = gizmosColor;
            Gizmos.DrawLine(_startPoint.position, _p1.position);
            var count = CurvePointsCount;
            _curveLength = 0;
            for (var i = 1; i < count; i++)
            {
                var prevT = (float)(i-1) / (count -1);
                var currT = (float)(i) / (count -1);
                var prevP = Bezier.GetPosition(_p1.position, _p2.position, _p3.position, prevT);
                var currP= Bezier.GetPosition(_p1.position, _p2.position, _p3.position, currT);
                _curveLength += (currP - prevP).magnitude;
                Gizmos.DrawLine(prevP, currP);
            }
            Gizmos.color = oldColor;
        }
        #endregion
        
        private void OnValidate()
        {
            RecalculateLength();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
                Skip();
        }
#endif
        
        public void RunCamera(CamFollower cam, Action returnCamera)
        {
            Stop();
            _onDone = returnCamera;
            _moving = StartCoroutine(Flying(cam.transform, returnCamera));
        }

        [ContextMenu("Stop moving")]
        public void Stop()
        {
            if(_moving!= null)
                StopCoroutine(_moving);
        }
        
        
        [ContextMenu("Calculate Length")]
        public void RecalculateLength()
        {
            if (_p1 == null || _p2 == null || _p3 == null)
                return;
            var count = CurvePointsCount;
            _curveLength = 0;
            for (var i = 1; i < count; i++)
            {
                var prevT = (float)(i-1) / (count -1);
                var currT = (float)(i) / (count -1);
                var prevP = Bezier.GetPosition(_p1.position, _p2.position, _p3.position, prevT);
                var currP= Bezier.GetPosition(_p1.position, _p2.position, _p3.position, currT);
                _curveLength += (currP - prevP).magnitude;
            }   
        }
        
        public void SetStartPos(CamFollower cam)
        {
            cam.transform.position = _startPoint.position;
            cam.transform.rotation = Quaternion.LookRotation(_lookAt.position- cam.transform.position);
        }
        
        private IEnumerator Flying(Transform movable, Action onDone)
        {
            movable.position = _startPoint.position;
            SetRot();
            yield return new WaitForSeconds(_moveStartDelay);

            var elapsed = 0f;
            var time = (movable.position - _p1.position).magnitude / _startToP1Speed;
            if (time > .01)
            {
                while (elapsed <= time)
                {
                    movable.position = Vector3.Lerp(_startPoint.position, _p1.position, elapsed / time);
                    SetRot();
                    elapsed += Time.deltaTime;
                    yield return null;
                }   
            }

            SetRot();
            yield return MovingOnCurve(movable, _p1.position, _p2.position, _p3.position);
 
            if (_moveFrontAndBack)
                yield return MovingOnCurve(movable, _p3.position, _p2.position, _p1.position);
            
            yield return new WaitForSeconds(_endCallDelay);
            onDone.Invoke();
            void SetRot()
            {
                movable.rotation = Quaternion.LookRotation(_lookAt.position - movable.position);
            } 
        }

        private IEnumerator MovingOnCurve(Transform movable, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var elapsed = Time.deltaTime;
            var time =  _curveLength / _curveSpeed;
            movable.position = p1;
            while (elapsed <= time)
            {
                movable.position = Bezier.GetPosition(p1, p2, p3, elapsed / time);
                SetRot();
                elapsed += Time.deltaTime;
                yield return null;
            }
            movable.position = p3;
            SetRot();
            void SetRot()
            {
                movable.rotation = Quaternion.LookRotation(_lookAt.position - movable.position);
            } 
        }
        
        public void Skip()
        {
            CLog.LogWHeader("PreyPackCamera", "Skipping Trajectory movement", "w");
            Stop();
            StopAllCoroutines();
            _onDone?.Invoke();
            _onDone = null;
        }

        

    }
}