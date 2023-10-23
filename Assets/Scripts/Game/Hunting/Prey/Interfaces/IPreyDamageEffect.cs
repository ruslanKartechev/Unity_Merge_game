using UnityEngine;

namespace Game.Hunting
{
    public interface IPreyDamageEffect
    {
        public void Damaged();
        public void Particles(Vector3 position);
    }
}