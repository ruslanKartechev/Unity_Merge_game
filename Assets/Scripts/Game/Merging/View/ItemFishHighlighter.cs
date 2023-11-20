using UnityEngine;

namespace Game.Merging.View
{
    public class ItemFishHighlighter : MonoBehaviour, IMergeItemHighlighter
    {
        [SerializeField] private ParticleSystem _particles;
      
        
        public void Highlight()
        {
            _particles.gameObject.SetActive(true);
            _particles.Play();
        }

        public void Normal()
        {
            _particles.gameObject.SetActive(false);
        }
    }
}