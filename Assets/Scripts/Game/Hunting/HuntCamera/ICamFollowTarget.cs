using UnityEngine;

namespace Game.Hunting.HuntCamera
{
    public interface ICamFollowTarget
    {
        Vector3 GetPosition();
        Vector3 GetOffset();
        Transform GetPoint();
        Vector3 GetLookAtPosition();
        CameraSettings CameraSettings { get; }
    }
}