using System.Collections;
using Dreamteck.Splines;
using UnityEngine;

namespace Creatives.Office
{
    public class CameraStopper : MonoBehaviour
    {
        public bool autoStart;
        public Transform point;
        public SplineComputer spline;
        public GameObject camera;
        private Coroutine _working;
        
        private void Start()
        {
            if(autoStart)
                Begin();
        }

        public void Begin()
        {
            Stop();
            _working = StartCoroutine(Working());
        }

        public void Stop()
        {
            if(_working != null)
                StopCoroutine(_working);
        }

        private void Activate()
        {
            camera.transform.parent = null;
            var ss = camera.GetComponent<OfficeSplineCamera>();
            if(ss != null)
                ss.Stop();
        }
        
        private IEnumerator Working()
        {
            var res = new SplineSample();
            spline.Project(res, point.position);
            var threshold = .5f / 100f;
            var targetPercent = (float)res.percent;
            var cam = camera.transform;
            while (true)
            {
                spline.Project(res, cam.position);
                var percent = (float)res.percent;
                var diff = Mathf.Abs(percent - targetPercent);
                // Debug.Log($"Diff {diff * 100f}, threshold: {threshold * 100f}");
                if (diff < threshold || percent > targetPercent)
                {
                    Activate();
                    yield break;
                }
                yield return null;
            }
        }
    }
}