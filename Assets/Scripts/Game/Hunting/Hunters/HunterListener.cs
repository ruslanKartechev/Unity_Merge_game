using UnityEngine;

namespace Game.Hunting
{
    public abstract class HunterListener : MonoBehaviour
    {
        public abstract void OnAttack();
        public abstract void OnFall();
        public abstract void OnHitEnemy();
    }
}