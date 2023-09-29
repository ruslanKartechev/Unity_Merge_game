using System;

namespace Game.Shop
{
    public interface IShopEgg
    {
        void Crack(Action onEnd);
        void Reset();
    }
}