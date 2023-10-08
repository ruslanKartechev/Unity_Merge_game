using Game.UI.Merging;

namespace Game.Hunting.UI
{
    public interface IHuntUIPage
    {
        void UpdateMoney(float addedSum = 0);
        void SetKillCount(int killed, int target);
        void Win(float award);
        void Fail();
        ISuperEggUI SuperEggUI { get; }
        void Darken();
        void ShowPower(float ourPower, float enemyPower, float duration);
    }
    
}