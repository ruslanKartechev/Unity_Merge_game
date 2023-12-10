using Creatives.Office;
using UnityEngine;

namespace Creatives.StreetRunaway
{
    public class FireHydrantHitTarget : MonoBehaviour, IHitTarget
    {
        [SerializeField] private GameObject _go;
        [SerializeField] private ParticleSystem _particles;
        
        public void OnHit()
        {
            _particles.transform.parent = null;
            _particles.gameObject.SetActive(true);
            _particles.Play();
            _go.SetActive(false);
        }
    }
}