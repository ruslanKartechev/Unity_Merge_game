using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapEnemyTerritoryProps : MonoBehaviour
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