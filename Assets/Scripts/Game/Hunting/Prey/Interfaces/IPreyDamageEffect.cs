using UnityEngine;

namespace Game.Hunting.Prey.Interfaces
{
    public interface IPreyDamageEffect
    {
        public void Damaged();
        public void Particles(Vector3 position);
    }
}