using UnityEngine;

namespace Game.Hunting
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] private GameObject _go;

        public void Drop()
        {
            _go.SetActive(false);
        }
    }
}