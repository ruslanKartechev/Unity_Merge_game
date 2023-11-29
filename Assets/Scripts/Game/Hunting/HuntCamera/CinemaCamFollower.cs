using Cinemachine;
using UnityEngine;

namespace Game.Hunting.HuntCamera
{
    public class CinemaCamFollower : MonoBehaviour, ICamFollower, IJumpCamera
    {
        [SerializeField] private CinemachineBrain _cinemachine;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        
        public void FollowAndLook(ICamFollowTarget moveTarget, ICamFollowTarget lookTarget, bool warpTo = false)
        {
            _virtualCamera.enabled = false;
            if (warpTo)
            {
                _virtualCamera.transform.position = moveTarget.GetPoint().position;
                _virtualCamera.transform.rotation = Quaternion.LookRotation(lookTarget.GetPoint().position - moveTarget.GetPoint().position);
            }
            _virtualCamera.enabled = true;
            _virtualCamera.LookAt = lookTarget.GetPoint();
            _virtualCamera.Follow = moveTarget.GetPoint();
        }

        public void FollowOne(ICamFollowTarget target)
        {
            
        }

        public void FollowFromBehind(ICamFollowTarget target)
        {
            
        }
        
        public void FollowInJump(ICamFollowTarget target, Vector3 position)
        {
            
        }
        
        public Transform GetTransformToRun()
        {
            return transform;
        }

    }
}