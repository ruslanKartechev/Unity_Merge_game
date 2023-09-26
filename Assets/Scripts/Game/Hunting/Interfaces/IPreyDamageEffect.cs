using UnityEngine;

namespace Game.Hunting
{
    public interface IPreyDamageEffect
    {
        public void PlayDamaged();
        public void PlayAt(Vector3 position);
    }
}