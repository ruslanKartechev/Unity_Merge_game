using Dreamteck.Splines;
using UnityEngine;

namespace Creatives.Office
{
    public static class SplineHelper
    {
        public static void SetOffset(SplineFollower follower)
        {
            var res = new SplineSample();
            var mPos = follower.transform.position;
            follower.Project(mPos, res);
            var vec = mPos - res.position;
            var xOffset = Vector3.Dot(vec, res.right);
            follower.motion.offset = new Vector2(xOffset, 0f);
            follower.SetPercent(res.percent);
        }
    }
}