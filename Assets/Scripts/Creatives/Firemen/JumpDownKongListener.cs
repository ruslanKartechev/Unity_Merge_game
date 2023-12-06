using UnityEngine;

namespace Creatives.Firemen
{
    public abstract class JumpDownKongListener : MonoBehaviour
    {
        public abstract void OnLanded(Vector3 position);
    }
}