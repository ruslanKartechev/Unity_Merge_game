using System;

namespace Game.Hunting
{
    public interface IPreyHealth : IPredatorTarget
    {
        event Action OnDead;
        void Init(float maxHealth);
        void Show();
        void Hide();
        
    }
}