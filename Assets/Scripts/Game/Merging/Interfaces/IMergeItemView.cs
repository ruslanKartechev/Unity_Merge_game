using UnityEngine;

namespace Game.Merging
{
    public interface IMergeItemView
    {
        void SetPositionRotation(Vector3 position, Quaternion rotation);
        void OnSpawn();
        MergeItem Item { get; set; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        void Destroy();
        void SnapToPos(Vector3 position);
        void OnPicked();
        void OnReleased();
    }
}