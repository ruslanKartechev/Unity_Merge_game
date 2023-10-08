using UnityEngine;

namespace Game.Shop
{
    [CreateAssetMenu(menuName = "SO/Shop/" + nameof(ShopItemView), fileName = nameof(ShopItemView), order = 8)]
    public class ShopItemView : ScriptableObject, IShopItemView
    {
        public string id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _backgroundColor;
        [SerializeField] private string _label;
        [SerializeField] private Texture _renderTexture;
        [SerializeField] private GameObject _itemPrefab;
            
        public Sprite Sprite => _icon;
        public Color BackgroundColor => _backgroundColor;
        public string DisplayedName => _label;
            
        public GameObject Prefab => _itemPrefab;
        public Texture RenderTexture => _renderTexture;
    }
}