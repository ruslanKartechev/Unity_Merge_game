using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class LandHunterMouth : HunterMouth
    {
        [SerializeField] private float _distance = 1f;
        [SerializeField] private Vector3 _localPrePosition;
        [Space(10)]
        [SerializeField] private Joint _headJoint;
        [SerializeField] private Rigidbody _headRb;
        [Space(10)]
        [SerializeField] private RagdollPositioner _ragdollPositioner;
        [Space(10)] 
        [SerializeField] private HunterCamTargetMover _lookTargetMover;

        private const float UpOffset = 0.25f;
        
        public override void BiteTo(Transform movable, Transform parent, Transform refPoint, Vector3 position)
        {
            var joint = transform;
            _lookTargetMover?.Follow();
            position = parent.InverseTransformPoint(position);
            var planePoint = new Vector2(position.x, position.z).normalized * _distance;
            // var proj = Vector3.Dot(movable.position, parent.forward);
            // Debug.Log($"Projection: {proj}");
            // if (proj > 0)
            //     _localPrePosition.z *= -1f;
            //
            joint.parent = parent;
            movable.position = refPoint.TransformPoint(_localPrePosition);
            movable.rotation = refPoint.rotation;
            
            // var localPos = parent.InverseTransformPoint(refPoint.position);
            // var planePos = new Vector2(localPos.x, localPos.z) * _distance;
            // localPos = new Vector3(planePos.x, localPos.y, planePos.y);
            var rotVec = parent.position - (joint.position - joint.forward * 10f);
            rotVec.y = 0;
            joint.rotation = Quaternion.LookRotation(rotVec);
            // transform.localPosition = localPos;
            
            joint.localPosition = new Vector3(planePoint.x, joint.localPosition.y + UpOffset, planePoint.y);

            _headJoint.connectedBody = _headRb;
            _ragdollPositioner?.SetPosition();
            
        }
    }
    
}