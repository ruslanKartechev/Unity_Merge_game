using System;

namespace Game.Hunting.Hunters.Interfaces
{
    public interface IHunterJumper
    {
        public event Action OnAttackFinished;
        void Init(IHunterSettings settings, IPrey prey);
        void Jump(AimPath path);
    }
}