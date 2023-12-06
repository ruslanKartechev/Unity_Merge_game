using Common;
using UnityEngine;

namespace Creatives.Firemen
{
    public class KongLandEffects : JumpDownKongListener
    {
        [SerializeField] private CameraShakeArgs _shakeArgs;
        [SerializeField] private GameObject _cameraShaker;
        [SerializeField] private ParticleSystem _landParticles;

        public override void OnLanded(Vector3 position)
        {
            if (_landParticles != null)
            {
                _landParticles.transform.position = position;
                _landParticles.gameObject.SetActive(true);
                _landParticles.Play();
            }

            if (_cameraShaker.TryGetComponent<ICameraShaker>(out var shaker))
            {
                shaker.Play(_shakeArgs);
            }
        }
    }
}