using UnityEngine;

namespace Game.Hunting.HuntCamera
{
    [CreateAssetMenu(menuName = "SO/" + nameof(CameraSettings), fileName = nameof(CameraSettings), order = 0)]
    public class CameraSettings : ScriptableObject
    {
        public float moveToTargetSpeed = 1;
        public float lerpFollowSpeed = 0.1f;
        public float lerpRotSpeed = 0.1f;
        public float followUpOffsetSetTime = 0.5f;
        public float followUpOffsetMax = .8f;
        public bool maintainY = true;
        [Header("Side Movement")] 
        public Vector2 sideOffsetLimits;
        public float SideOffsetMoveTime;
        [Header("Forward Movement")] 
        public Vector2 forwardOffsetLimits;
        public float forwardOffsetMoveTime;
    }
}