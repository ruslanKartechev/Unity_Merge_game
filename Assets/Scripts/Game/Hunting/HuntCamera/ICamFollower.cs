using UnityEngine;

namespace Game.Hunting.HuntCamera
{
    public interface ICamFollower
    {
        void FollowAndLook(ICamFollowTarget moveTarget, ICamFollowTarget lookTarget, bool warpTo = false);
        void FollowOne(ICamFollowTarget target);
        void FollowFromBehind(ICamFollowTarget target);
        Transform GetTransformToRun();
    }
}