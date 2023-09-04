using UnityEngine;

namespace Game.Hunting.HuntCamera
{
    public interface ICamFollowTarget
    {
        Vector3 GetPosition();
        Vector3 GetOffset();
        Vector3 LocalToWorld(Vector3 vector);
    }
}