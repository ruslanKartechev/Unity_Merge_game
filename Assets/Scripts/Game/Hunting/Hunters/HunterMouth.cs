using UnityEngine;

namespace Game.Hunting.Hunters
{
    public abstract class HunterMouth : MonoBehaviour
    {
        public abstract void BiteTo(Transform movable, Transform parent, Transform refPoint, Vector3 position);
    }
}