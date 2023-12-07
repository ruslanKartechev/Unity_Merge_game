using Common;
using UnityEngine;

namespace Game.Hunting
{
    public class AimPath
    {
        public Vector3 start;
        public Vector3 end;
        public Vector3 inflection;

        public Vector3 GetPos(float t)
        {
            return Bezier.GetPosition(start, inflection, end, t);
        }
    }
}