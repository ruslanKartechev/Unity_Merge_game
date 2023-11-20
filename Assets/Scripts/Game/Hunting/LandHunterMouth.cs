using Game.Hunting.Hunters;
using UnityEngine;

namespace Game.Hunting
{
    public class LandHunterMouth : HunterMouth
    {
        [SerializeField] private Joint _headJoint;
        [SerializeField] private Rigidbody _headRb;
        [Space(10)]
        [SerializeField] private RagdollPositioner _ragdollPositioner;
        
        public override void BiteTo(Transform movable, Transform parent, Transform refPoint, Vector3 position)
        {
            var joint = transform;
            var rotVec = (parent.position - position);
            var joint_pos = position;
            var joint_rot = Quaternion.LookRotation(rotVec);
            joint.SetPositionAndRotation(joint_pos, joint_rot);
            joint.SetParent(parent);
            _headJoint.connectedBody = _headRb;
            _ragdollPositioner?.SetPosition();
        }

    }
    
}