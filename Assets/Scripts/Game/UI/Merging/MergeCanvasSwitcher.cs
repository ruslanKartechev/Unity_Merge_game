using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeCanvasSwitcher : MonoBehaviour
    {
        [SerializeField] private Canvas _shop;
        [SerializeField] private Canvas _main;
        [SerializeField] private Canvas _stats;
        
        public void Shop()
        {
            _stats.enabled = true;
            _main.enabled = false;
            _shop.enabled = true;
            _shop.gameObject.SetActive(true);
        }

        public void Main()
        {
            _stats.enabled = true;
            _main.enabled = true;
            // _shop.enabled = false;
            _shop.gameObject.SetActive(false);
        }
    }
}