using Dreamteck.Splines;
using UnityEngine;

namespace Creatives.Office
{
    public class RunawayHuman : MonoBehaviour
    {
        public HumanAnim runAnim;
        public OfficeHuman human;
        public SplineFollower splineFollower;

        private void Start()
        {
            human.OnDead += OnDead;
            SplineHelper.SetOffset(splineFollower);
            splineFollower.enabled = true;
            splineFollower.follow = true;
            runAnim.Play(human.animator);
        }

        private void OnDead()
        {
            splineFollower.enabled = false;
            human.DollDie();
        }
        
    }
}