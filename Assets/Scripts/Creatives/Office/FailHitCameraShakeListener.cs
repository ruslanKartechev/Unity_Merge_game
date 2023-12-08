using Common;
using UnityEngine;

namespace Creatives.Office
{
    public class FailHitCameraShakeListener : MonoBehaviour, ICreosAnimalListener
    {
        public CameraShaker shaker;
        public CameraShakeArgs shakeArgs;
        
        public void OnFailHit()
        {
            shaker.Play(shakeArgs);
        }
    }
}