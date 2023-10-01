using Game.Hunting;
using Game.Hunting.UI;

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