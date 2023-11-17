using UnityEngine;

namespace Game.UI.Merging
{
    public class MergeAreaSliderController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.ScrollRect _scroll;
        [SerializeField] private GameObject _sliderGo;

        public void Enable()
        {
            _scroll.enabled = true;
            // _sliderGo.SetActive(true);
        }

        public void Disable()
        {
            _scroll.enabled = false;
            // _sliderGo.SetActive(false);   
        }
    }
}