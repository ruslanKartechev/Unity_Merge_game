using Common;
using Game.UI.Elements;
using Game.UI.Merging;

namespace Game.Hunting.UI
{
    public interface IHuntUIPage
    {
        void SetKillCount(int killed, int target);
        ISuperEggUI SuperEggUI { get; }
        IFlyingMoney FlyingMoney { get; }
        void ShowPower(float ourPower, float enemyPower, float duration);
        ProperButton InputButton { get; }
        
    }
    
}