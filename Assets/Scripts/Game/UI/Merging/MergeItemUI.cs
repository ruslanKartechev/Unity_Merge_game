using Game.Merging;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeItemUI : MonoBehaviour, IMergeItemUI
    {
        [SerializeField] private Image _icon;
        [SerializeField] private MergeItemUILevel _levelUI;
        private MergeItem _item;

        public MergeItem Item
        {
            get => _item;
            set => _item = value;
        }
        
        [ContextMenu("ShowEmpty()")]
        public void SetEmpty()
        {
            _levelUI.Hide();
            _icon.enabled = false;
        }

        [ContextMenu("ShowItemData()")]
        public void ShowItemData()
        {
            _levelUI.Show();
            _icon.enabled = true;
            _icon.sprite = null;
            _levelUI.SetLevel(_item.level);
        }

        public void PlayMerged()
        {
            _levelUI.PlayScale();   
        }
    }
}