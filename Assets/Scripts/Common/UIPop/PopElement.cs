using UnityEngine;

namespace Common.UIPop
{
    public abstract class PopElement : MonoBehaviour
    {
        public abstract float Delay { get; }
        public abstract float Duration { get; }

        public abstract void ScaleUp();
    }
}