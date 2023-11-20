using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting.Prey
{
    public class PreyPackCameraTrajectoryTester : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private CamFollower _camera;
        [SerializeField] private CameraFlyOver _trajectory;
        private void OnValidate()
        {
            if (_camera == null)
            {
                _camera = FindObjectOfType<CamFollower>();
            }

            if (_trajectory == null)
                _trajectory = GetComponent<CameraFlyOver>();
        }

        [ContextMenu("Test trajectory")]
        public void StartTest()
        {
            _trajectory.RunCamera(_camera, () =>
            {
                Debug.Log("Test completed");
            });
        }

        [ContextMenu("Set Start Pos")]
        public void SetStartPos()
        {
            _trajectory.SetStartPos(_camera);
        }
        
#endif
    }
}