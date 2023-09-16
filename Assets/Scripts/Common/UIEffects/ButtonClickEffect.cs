using UnityEngine;

namespace Common.UIEffects
{
    public abstract class ButtonClickEffect : MonoBehaviour
    {
        [SerializeField] protected RectTransform _target;
        public abstract void Play();
    }
}