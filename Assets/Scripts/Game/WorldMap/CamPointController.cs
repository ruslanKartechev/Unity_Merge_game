using UnityEngine;

namespace Game.WorldMap
{
    public class CamPointController : MonoBehaviour
    {
#if UNITY_EDITOR
        public Transform lookAtPoint;
        
        public void SetCamToThisPoint()
        {
            var cam = Camera.main.transform;
            cam.transform.position = transform.position;
            cam.rotation = Quaternion.LookRotation(lookAtPoint.position - transform.position);
        }
        
#endif
    }
}