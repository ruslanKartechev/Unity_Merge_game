using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapEnemyProps : MonoBehaviour
    {

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}