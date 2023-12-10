using UnityEngine;

namespace Creatives.Gozilla
{
    public class GodzillaAnimEvent : MonoBehaviour
    {
        [SerializeField] private GodzillaSolo _godzilla;

        public void OnJump()
        {
            _godzilla.Attack();
        }
    }
}