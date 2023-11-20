using Game.Hunting;
using Game.Hunting.Prey.Interfaces;
using Game.UI.Hunting;

namespace Game.Levels
{
    public interface IRewardCalculator
    {
        void Init(IPreyPack pack, IHuntUIPage ui);
        public void ResetReward();
        public void ApplyReward();
        float TotalReward { get; }
    }
}