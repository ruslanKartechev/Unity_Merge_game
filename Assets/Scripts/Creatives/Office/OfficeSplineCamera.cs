using System.Collections;
using Dreamteck.Splines;
using UnityEngine;

namespace Creatives.Office
{
    public class OfficeSplineCamera : MonoBehaviour
    {
        public float percentOffset;
        public Vector3 offset;
        public Transform follow;
        public Transform pack;
        public SplineComputer spline;
        public bool autoStart = true;
        private Coroutine _working;
        
        private void Start()
        {
            if(autoStart)
                Activate();
        }

        public void Activate()
        {
            if(_working != null)
                StopCoroutine(_working);
            _working = StartCoroutine(Working());
        }

        private IEnumerator Working()
        {
            while (true)
            {
                var pos = follow.position;
                var percent = spline.Project(pack.position).percent;
                percent += percentOffset / 100f;
                var lookRes = spline.Evaluate(percent);
                var lookPos = lookRes.position 
                              + lookRes.forward * offset.z + lookRes.up * offset.y + lookRes.right * offset.x;
                var lookVec = lookPos - pos;
                var rot = Quaternion.LookRotation(lookVec);
                transform.SetPositionAndRotation(pos, rot);
                yield return null;
            }
        }
    }
}