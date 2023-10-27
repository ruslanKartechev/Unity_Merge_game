using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapCameraPoint : MonoBehaviour
    {
        [SerializeField] private Transform _lookAtPoint;
        [SerializeField] private Transform _pointFar;
        [SerializeField] private Transform _pointClose;

        public Transform LookAt => _lookAtPoint;
        public Transform PointFar => _pointFar;
        public Transform PointClose => _pointClose;


        #if UNITY_EDITOR

        public void SetCamToFarPoint()
        {
            var cam = FindObjectOfType<MapCamera>();
            if (!cam)
                return;
            var pos = _pointFar.position;
            var rot = Quaternion.LookRotation(_lookAtPoint.position - pos);
            cam.SetPosRot(pos, rot);
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        public void SetCamToClosePoint()
        {
            var cam = FindObjectOfType<MapCamera>();
            if (!cam)
                return;
            var pos = _pointClose.position;
            var rot = Quaternion.LookRotation(_lookAtPoint.position - pos);
            cam.SetPosRot(pos, rot);
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void SetRotToLook()
        {
            var cam = FindObjectOfType<MapCamera>();
            if (!cam)
                return;
            cam.transform.rotation = Quaternion.LookRotation(_lookAtPoint.position - cam.transform.position);
        }

        public void SaveFarPoint()
        {
            var cam = FindObjectOfType<MapCamera>();
            if (!cam)
                return;
            _pointFar.position = cam.transform.position;
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void SaveClosePoint()
        {
            var cam = FindObjectOfType<MapCamera>();
            if (!cam)
                return;
            _pointClose.position = cam.transform.position;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
        
    }
}