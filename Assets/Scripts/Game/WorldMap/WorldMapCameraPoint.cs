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
        [Space(10)]
        [SerializeField] private WorldMapCameraPoint _copyFrom;
        [SerializeField] public bool doDraw;

        private CamPointController _camPointControllerClose;
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

        [ContextMenu("Copy")]
        public void CopyFrom()
        {
            if (_copyFrom == null)
                return;
            _lookAtPoint.localPosition = _copyFrom._lookAtPoint.localPosition;
            _lookAtPoint.localRotation = _copyFrom._lookAtPoint.localRotation;
            
            _pointFar.localPosition = _copyFrom._pointFar.localPosition;
            _pointFar.localRotation = _copyFrom._pointFar.localRotation;
            
            _pointClose.localPosition = _copyFrom._pointClose.localPosition;
            _pointClose.localRotation = _copyFrom._pointClose.localRotation;
        }

        public void Draw()
        {
            Debug.DrawLine(_pointClose.position, _lookAtPoint.position, Color.blue, 1f);
        }

        public void TrueGetCamPoints()
        {
            if (_camPointControllerClose == null){
                _camPointControllerClose = _pointClose.GetComponent<CamPointController>();
                if(_camPointControllerClose == null)
                    _camPointControllerClose = _pointClose.gameObject.AddComponent<CamPointController>();
            }
            if (_camPointControllerClose != null)
                _camPointControllerClose.lookAtPoint = _lookAtPoint;
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