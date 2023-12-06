using Common;
using UnityEngine;

namespace Creatives.Kong
{
    public class ThrowHumanCollisionTrigger : MonoBehaviour
    {
        [SerializeField] private string _collisionObjectName;
        [SerializeField] private CameraShakeArgs _shakeArgs;
        [SerializeField] private GameObject _camera;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == _collisionObjectName)
            {
                Debug.Log("****** TARGET FOUND");
                if (_camera != null)
                {
                    if (_camera.TryGetComponent<ICameraShaker>(out var shaker))
                    {
                        shaker.Play(_shakeArgs);
                    }
                }
                gameObject.SetActive(false);
            }
        }
    }
}