using UnityEngine;

namespace Common.Ragdoll
{
    public abstract class IRagdoll : MonoBehaviour
    {
        public abstract void Deactivate();
        public abstract void Activate();
        public abstract void ActivateAndPush(Vector3 force);
        public abstract bool IsActive { get; protected set; }

    }
}