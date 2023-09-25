using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class LandHunterMouth : HunterMouth
    {
        [SerializeField] private float _distance = 1f;
        [Space(10)]
        [SerializeField] private Joint _headJoint;
        [SerializeField] private Rigidbody _headRb;
        [Space(10)]
        [SerializeField] private RagdollPositioner _ragdollPositioner;
        [Space(10)] 
        [SerializeField] private List<Transform> _reparentTargets;
        
        public override void BiteTo(Transform movable, Transform parent, Transform refPoint)
        {
            foreach (var target in _reparentTargets)
                target.parent = parent;
            
            movable.position = refPoint.TransformPoint(new Vector3(0f, -1f, -1f));
            movable.rotation = refPoint.rotation;
            
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