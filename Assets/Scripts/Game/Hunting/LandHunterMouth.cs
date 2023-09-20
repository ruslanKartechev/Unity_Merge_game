using UnityEngine;

namespace Game.Hunting
{
    public class LandHunterMouth : HunterMouth
    {
        [SerializeField] private float _distance = 1f;
        [Space(10)]
        [SerializeField] private Joint _headJoint;
        [SerializeField] private Joint _leftJoint;
        [SerializeField] private Joint _rightJoint;
        [Space(10)]
        [SerializeField] private Rigidbody _headRb;
        [SerializeField] private Rigidbody _leftRb;
        [SerializeField] private Rigidbody _rightRb;
        [Space(10)]
        [SerializeField] private RagdollPositioner _ragdollPositioner;
        
        public override void BiteTo(Transform parent, Transform refPoint)
        {
            transform.parent = parent;
            var localPos = parent.InverseTransformPoint(refPoint.position);
            var planePos = new Vector2(localPos.x, localPos.z) * _distance;
            localPos = new Vector3(planePos.x, localPos.y, planePos.y);
            var rotVec = parent.position - transform.position;
            rotVec.y = 0;
            transform.rotation = Quaternion.LookRotation(rotVec);
            transform.localPosition = localPos;
            _headJoint.connectedBody = _headRb;
            _ragdollPositioner?.SetPosition();

            // _leftJoint.connectedBody = _leftRb;
            // _rightJoint.connectedBody = _rightRb;
        }
    }
    
}