using UnityEngine;

namespace Game.Hunting.HuntCamera
{
    public interface ICamFollowTarget
    {
        Vector3 GetPosition();
        Vector3 GetOffset();

        Vector3 GetLookAtPosition();
        // Vector3 GetLookOffset();
        // Vector3 LocalToWorld(Vector3 position);
        // Vector3 WorldToLocal(Vector3 position);
        CameraSettings CameraSettings { get; }
    }
}