using Game.Merging;
using UnityEngine;

namespace Game.UI.Merging
{
    public class ActiveGroupCell : MonoBehaviour, IGroupCell
    {
        [SerializeField] private MergingCellCostDisplay _costDisplay;
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private ParticleSystem _spawnedParticles;
        private bool _isAvailable;
        private bool _isFree;
        
        public int X { get; set; }
        public int Y { get; set; }
        public float Cost { get; set; }
        public bool IsFree => _isFree;
        public bool IsPurchased => _isAvailable;
        private IActiveGroupCell _activeGroupCell;
        private IMergeItemView _currentItem;
        
        public void SetForSale(float cost)
        {
            _isFree = false;
            Cost = cost;
            _isAvailable = false;
            _costDisplay.SetCost(cost);
            _costDisplay.Show();
            _collider.enabled = true;
        }

        public void SetAvailable()
        {
            _isFree = true;
            _isAvailable = true;
            _costDisplay.Hide();
            _collider.enabled = true;
        }

        public void Purchase()
        {
            _activeGroupCell.Purchased = true;
            _costDisplay.Hide();
            _isAvailable = true;
            _isFree = true;
        }

        public void SpawnItem(IMergeItemView itemView, MergeItem item)
        {
            _isFree = false;
            _activeGroupCell.Item = item;
            _currentItem = itemView;
            itemView.Item = item;
            itemView.SetPositionRotation(_spawnPoint.position, _spawnPoint.rotation);
            itemView.OnSpawn();
            _spawnedParticles.Play();
        }

        public void PutItem(IMergeItemView item)
        {
            _isFree = false;
            _activeGroupCell.Item = item.Item;
            _currentItem = item;
            _currentItem.SnapToPos(_spawnPoint.position);
        }

        public IMergeItemView TakeItem()
        {
            var item = _currentItem;
            RemoveItem();
            return item;
        }

        public IMergeItemView GetItem()
        {
            return _currentItem;
        }

        public void RemoveItem()
        {
            _currentItem = null;
            _isFree = true;
            _activeGroupCell.Item = null;
        }

        public void Init(IActiveGroupCell data)
        {
            _activeGroupCell = data;
            if(data.Purchased)
                SetAvailable();
            else
                SetForSale(data.Cost);
        }

        public void SetInactive()
        {
            _isAvailable = false;
            _costDisplay.Hide();
            _collider.enabled = false;
        }

    
    }
}