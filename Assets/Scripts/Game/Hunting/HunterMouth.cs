using UnityEngine;

namespace Game.Hunting
{
    public class HunterMouth : MonoBehaviour
    {
        public Joint joint;

        public void BiteTo(Transform parent, Rigidbody rb)
        {
            transform.parent = parent;
            transform.rotation = Quaternion.LookRotation(parent.position - transform.position);
            joint.connectedBody = rb;
        }
    }
    
}