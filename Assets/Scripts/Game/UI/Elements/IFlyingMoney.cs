using UnityEngine;

namespace Game.UI.Elements
{
    public interface IFlyingMoney
    {
        void FlySingle();
        void FlySingle(Vector3 fromPosition);
    }
}