using System;
using Game.Merging;

namespace Game.Hunting
{
    public interface IHunterJumper
    {
        public event Action OnAttackFinished;
        void Init(IHunterSettings settings, IPrey prey);
        void Jump(AimPath path);
    }
}