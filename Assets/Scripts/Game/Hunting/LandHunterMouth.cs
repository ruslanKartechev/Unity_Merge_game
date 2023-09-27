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
        private const float BodyPosDistance = 0.6f;

        
        public override void BiteTo(Transform movable, Transform parent, Transform refPoint, Vector3 position)
        {
            var joint = transform;
            _lookTargetMover?.Follow();
            position = parent.InverseTransformPoint(position);
            var planePoint = new Vector2(position.x, position.z).normalized * _distance;

            movable.position = parent.TransformPoint(parent.InverseTransformPoint(movable.position).normalized * BodyPosDistance);
            movable.rotation = refPoint.rotation;
            
            joint.parent = parent;
            var rotVec = parent.position - (joint.position - joint.forward * 10f);
            rotVec.y = 0;
            joint.rotation = Quaternion.LookRotation(rotVec);
            
            joint.localPosition = new Vector3(planePoint.x, joint.localPosition.y + UpOffset, planePoint.y);

            _headJoint.connectedBody = _headRb;
            _ragdollPositioner?.SetPosition();
            
        }
    }
    
}