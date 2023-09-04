using UnityEngine;

namespace Game.Merging
{
    public interface IMergeItem
    {
        void SetPositionRotation(Vector3 position, Quaternion rotation);
        void OnSpawn();
        int ItemLevel { get; }
        Vector3 GetPosition();
        void SetPosition(Vector3 position);
        void Destroy();
        void SnapToPos(Vector3 position);
        void OnPicked();
        void OnReleased();
    }
}