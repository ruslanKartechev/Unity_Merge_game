using System;

namespace Game.Hunting.UI
{
    public interface IHuntUIPage
    {
        void UpdateMoney();
        void SetKillCount(int killed, int target);
        void Win(float award);
        void Fail();
        void HidePopup(Action onEnd);
    }
    
}