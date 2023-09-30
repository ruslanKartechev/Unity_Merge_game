using UnityEngine;

namespace Game.Hunting
{
    public class PreyPackTrigger : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _pack;
        
        public void Activate(bool active)
        {
            gameObject.SetActive(active);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<IHunter>() != null)
            {
                ((IPreyPack)_pack).RunAttacked();
                Activate(false);
            }
        }
    }
}