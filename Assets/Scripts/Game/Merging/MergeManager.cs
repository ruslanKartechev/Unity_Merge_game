using Common;
using Game.UI.Merging;
using UnityEngine;
using Utils;

namespace Game.Merging
{
    public class MergeManager : MonoBehaviour
    {
        [SerializeField] private CameraPoint _cameraPoint;
        [SerializeField] private MergingGridSpawner _gridSpawner;
        [SerializeField] private MergeGridRepository _mergeRepository;
        private IMergingPage _mergingPage;
        private IItemPurchaser _itemPurchaser;
        private IMergeInput _mergeInput;
        private IMergeItemSpawner _itemSpawner;
        private bool _canPlay = false;
        
        private void Awake()
        {
            _mergeInput = GetComponent<IMergeInput>();
            _itemPurchaser = GetComponent<IItemPurchaser>();
            _itemSpawner = GetComponent<IMergeItemSpawner>();
        }

        public void Init(IMergingPage page)
        {
            _mergingPage = page;
            CameraPointMover.SetToPoint(_cameraPoint);
            _mergingPage.SetPurchaseCost(_itemPurchaser.GetCost());
            var cells = _gridSpawner.Spawn(_mergeRepository.GetSetup(), _itemSpawner);
            _itemPurchaser.Init(cells, _itemSpawner, _mergingPage);
            _mergeInput.Init(_mergingPage, _itemSpawner);
            _canPlay = CheckPlayable();
            if(_canPlay == false)
                _mergingPage.HidePlayButton();
            LoadingCurtain.Open(() =>{_mergeInput.Activate();});
        }
        
        public void OnPlayButton()
        {
            if (CheckPlayable())
            {
                CLog.LogWHeader(nameof(MergeManager), "Proceed to hunting mode", "w");
                LoadingCurtain.Close(() =>
                {
                    Container.LevelManager.LoadCurrent();
                });
            }
            else
            {
                CLog.LogWHeader(nameof(MergeManager), "Cannot play! Need to get hunters", "w");
            }
        }

        public void OnPurchase()
        {
            if (_itemPurchaser.PurchaseNewItem())
            {
                _mergingPage.UpdateMoney();
                if (!_canPlay)
                {
                    _canPlay = CheckPlayable();
                    if(_canPlay)
                        _mergingPage.ShowPlayButton(true);
                }
            }
        }

        private bool CheckPlayable()
        {
            var setup =_mergeRepository.GetSetup();
            for (var i = 0; i < setup.RowsCount; i++)
            {
                var row = setup.GetRow(i);
                if (row.IsAvailable == false)
                    return false;
                for (var x = 0; x < row.CellsCount; x++)
                {
                    var cell = row.GetCell(x);
                    if (cell.Purchased && cell.SpawnItemLevel >= 0)
                        return true;
                }
            }
            return false;
        }

    }
}