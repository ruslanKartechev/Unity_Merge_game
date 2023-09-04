using UnityEngine;

namespace Game.Hunting.HuntCamera
{
    public class CamFollowTarget : MonoBehaviour, ICamFollowTarget
    {
        [SerializeField] private Transform _camTarget;
        [SerializeField] private Vector3 _offset;
        
        public Vector3 GetPosition() => _camTarget.position;
        public Vector3 GetOffset() => _offset;
        
        public Vector3 LocalToWorld(Vector3 vector)
        {
            return _camTarget.TransformVector(vector);
        }
    }
}