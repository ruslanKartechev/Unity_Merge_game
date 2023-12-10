using Common;
using UnityEngine;

namespace Creatives.Gozilla
{
    public class DiveCurve : MonoBehaviour
    {
        [SerializeField] private Transform _p1;
        [SerializeField] private Transform _p2;
        [SerializeField] private Transform _p3;

        public Transform P1 => _p1;
        public Transform P2 => _p2;
        public Transform P3 => _p3;
        

#if UNITY_EDITOR
        public bool doDraw = true;
        private void OnDrawGizmos()
        {
            if (!doDraw)
                return;
            var count = 20;
            var p1 = _p1.position;
            var p2 = _p2.position;
            var p3 = _p3.position;
            for (var i = 1; i <= count; i++)
            {
                var t1 = (float)(i-1) / count;
                var t2 = (float)i / count;
                Gizmos.DrawLine(Bezier.GetPosition(p1, p2, p3, t1),
                    Bezier.GetPosition(p1, p2, p3, t2));
            }
        }
#endif
    }
}