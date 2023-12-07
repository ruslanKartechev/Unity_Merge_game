using UnityEngine;

namespace Game.Hunting.Hunters
{
    public abstract class HunterMouth : MonoBehaviour
    {
        public abstract void BiteTo(Transform parent, Vector3 position);
    }
}