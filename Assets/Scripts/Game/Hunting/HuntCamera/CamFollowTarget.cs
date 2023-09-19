using System;
using UnityEngine;

namespace Game.Hunting.HuntCamera
{
    public class CamFollowTarget : MonoBehaviour, ICamFollowTarget
    {
        [SerializeField] private Transform _camTarget;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector3 _offsetLook;
#if UNITY_EDITOR
        public bool drawGizmos;
        public float gizmoSize;
        public Color gizmoColor;
#endif
        
        
        public Vector3 GetPosition() => _camTarget.position;
        public Vector3 GetOffset() => _offset;
        public Vector3 GetLookOffset() => _offsetLook;

        public Vector3 LocalToWorld(Vector3 position) => _camTarget.TransformPoint(position);
        public Vector3 WorldToLocal(Vector3 position) => _camTarget.InverseTransformPoint(position);

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;
            var oldColor = Gizmos.color;
            Gizmos.color = gizmoColor;
            var followPos = LocalToWorld(_offset);
            Gizmos.DrawSphere(followPos, gizmoSize);
            Gizmos.DrawLine(followPos, _camTarget.position + _offsetLook);
            Gizmos.color = oldColor;
        }
        #endif
    }
}