using UnityEngine;

namespace Game.Hunting
{
    public class LandHunterMouth : HunterMouth
    {
        [SerializeField] private Joint _headJoint;
        [SerializeField] private Joint _leftJoint;
        [SerializeField] private Joint _rightJoint;
        [Space(10)]
        [SerializeField] private Rigidbody _headRb;
        [SerializeField] private Rigidbody _leftRb;
        [SerializeField] private Rigidbody _rightRb;

        [Header("RotateTargets")] 
        [SerializeField] private Transform _leftRot;
        [SerializeField] private Transform _rightRot;
        [SerializeField] private Vector3 _leftAngles;
        [SerializeField] private Vector3 _rightAngles;
        
        
        public override void BiteTo(Transform parent, Vector3 position)
        {
            transform.parent = parent;
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(parent.position - transform.position);
            _headJoint.connectedBody = _headRb;
            _leftJoint.connectedBody = _leftRb;
            _rightJoint.connectedBody = _rightRb;
            _leftRot.localEulerAngles = _leftAngles;
            _rightRot.localEulerAngles = _rightAngles;
        }
    }
    
}