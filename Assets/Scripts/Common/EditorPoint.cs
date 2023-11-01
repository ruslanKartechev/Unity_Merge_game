using UnityEngine;

namespace Common
{
    public class EditorPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool doDraw = true;
        public bool drawDirection;
        public Color color = Color.green;
        public float radius;

        
        private void OnDrawGizmos()
        {
            if (!doDraw)
                return;
            var oldColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, radius);
            if (drawDirection)
                Gizmos.DrawRay(new Ray(transform.position ,transform.forward));
            Gizmos.color = oldColor;
        }

#endif
    }
}