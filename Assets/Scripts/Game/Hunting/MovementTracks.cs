using Dreamteck.Splines;

namespace Game.Hunting
{
    public class MovementTracks
    {
        public MovementTracks(SplineComputer main, SplineComputer water)
        {
            this.main = main;
            this.water = water;
        }

        public MovementTracks(SplineComputer main, SplineComputer water, float speed)
        {
            this.main = main;
            this.water = water;
            this.moveSpeed = speed;
        }
        
        public SplineComputer main;
        public SplineComputer water;
        public float moveSpeed;
        public float accelerationDuration = 1f;
    }
}