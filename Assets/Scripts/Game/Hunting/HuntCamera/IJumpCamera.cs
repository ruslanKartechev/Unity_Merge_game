using UnityEngine;

namespace Game.Hunting.HuntCamera
{
    public interface IJumpCamera
    {
        void FollowInJump(ICamFollowTarget target, Vector3 position);
    }
}