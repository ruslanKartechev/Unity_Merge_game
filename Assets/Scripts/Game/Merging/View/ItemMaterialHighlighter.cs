using UnityEngine;

namespace Game.Merging
{
    public class ItemMaterialHighlighter : MonoBehaviour, IMergeItemHighlighter
    {
        [SerializeField] private MaterialSwapper _swapper;

        public void Highlight()
        {
            _swapper.Switch();
        }

        public void Normal()
        {
            _swapper.ReturnNormal();
        }
    }
}