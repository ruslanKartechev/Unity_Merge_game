using UnityEngine;

namespace Game.Hunting
{
    public abstract class PreyActionListener : MonoBehaviour
    {
        public abstract void OnStarted();
        public abstract void OnDead();
        public abstract void OnBeganRun();

    }
}