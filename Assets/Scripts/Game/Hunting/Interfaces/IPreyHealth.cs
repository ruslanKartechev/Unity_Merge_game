using System;

namespace Game.Hunting
{
    public interface IPreyHealth : IBiteTarget
    {
        event Action OnDead;
        void Init(float maxHealth);
        void Show();
        void Hide();
        
    }
}