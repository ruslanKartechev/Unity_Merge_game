using UnityEngine;

namespace Common
{
    public class EditorPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private bool doDraw = true;
        [SerializeField] private Color color = Color.green;
        [SerializeField] private float radius;

        
        private void OnDrawGizmos()
        {
            if (!doDraw)
                return;
            var oldColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, radius);
            Gizmos.color = oldColor;
        }

#endif
    }
}