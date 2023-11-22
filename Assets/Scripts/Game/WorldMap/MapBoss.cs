using UnityEngine;

namespace Game.WorldMap
{
    [DefaultExecutionOrder(-1)]
    public class MapBoss : MonoBehaviour
    {
        [SerializeField] private WorldMapEnemyProps _props;
        private const float HideDuration = 1f;

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnConquered()
        {
            StartCoroutine(_props.AnimatingDown(HideDuration)); 
        }

   
    }
}