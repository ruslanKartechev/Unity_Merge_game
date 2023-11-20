using UnityEngine;

namespace Game.UI.Elements
{
    public interface IMoneyUI
    {

        void UpdateCount(bool animated = true);
        void Highlight();
        Vector3 GetFlyToPosition();
    }
}