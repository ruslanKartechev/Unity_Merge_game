using UnityEngine;

namespace Game.Hunting.HuntCamera
{
    [CreateAssetMenu(menuName = "SO/" + nameof(CameraSettings), fileName = nameof(CameraSettings), order = 0)]
    public class CameraSettings : ScriptableObject
    {
        public float moveToTargetSpeed = 1;
    }
}