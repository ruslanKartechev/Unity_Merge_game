using UnityEngine;

namespace Game.Merging
{
    public interface IMergeItemView
    {
        void SetPositionRotation(Vector3 position, Quaternion rotation);
        void SetSettings(IHunterSettings settings);
        void OnSpawn();
        MergeItem Item { get; set; }
        Quaternion Rotation { get; set; }
        void SetDraggedPosition(Vector3 position);
        void Destroy();
        void SnapToPos(Vector3 position);
        void OnPicked();
        void OnReleased();
        Vector3 GetModelPosition();
    }
}