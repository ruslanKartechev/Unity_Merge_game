using Dreamteck.Splines;

namespace Creatives.Office
{
    public static class SplineHelper
    {
        public static void SetOffset(SplineFollower follower)
        {
            var res = new SplineSample();
            follower.Project(follower.transform.position, res);
            follower.motion.offset = (res.position - follower.transform.position);
            
        }
    }
}