using UnityEngine;

namespace Game.Shop
{
    public interface IShopItemView
    {
        Sprite Sprite { get; }
        Color BackgroundColor { get; }
        string DisplayedName { get; }
    
        public GameObject Prefab  { get; }
        public Texture RenderTexture { get; }
        
    }
}