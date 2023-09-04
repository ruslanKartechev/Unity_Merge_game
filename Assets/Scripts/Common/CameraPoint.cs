using UnityEngine;

namespace Common
{
    public class CameraPoint : MonoBehaviour, ICameraPoint
    {
        #if UNITY_EDITOR
        public bool AutoSet = true;
        #endif
        public Transform GetPoint() => transform;
        
        
    }
}