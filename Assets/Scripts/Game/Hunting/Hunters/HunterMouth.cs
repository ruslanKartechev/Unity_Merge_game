using UnityEngine;

namespace Game.Hunting
{
    public abstract class HunterMouth : MonoBehaviour
    {
        public abstract void BiteTo(Transform movable, Transform parent, Transform refPoint, Vector3 position);
    }
}