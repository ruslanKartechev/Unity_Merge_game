using UnityEngine;

namespace Game.Hunting
{
    public class AimPath
    {
        public Vector3 start;
        public Vector3 end;
        public Vector3 inflection;

        public bool lockOnTarget;
        public Transform target;

        public Vector3 GetEndPos()
        {
            if (lockOnTarget)
                return target.position;
            return end;
        }
    }
}