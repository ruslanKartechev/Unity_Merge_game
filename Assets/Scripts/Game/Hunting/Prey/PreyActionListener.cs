using UnityEngine;

namespace Game.Hunting.Prey
{
    public abstract class PreyActionListener : MonoBehaviour
    {
        public abstract void OnInit();
        public abstract void OnDead();
        public abstract void OnBeganRun();

    }
}