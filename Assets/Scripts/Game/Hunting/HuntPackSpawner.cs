using System.Collections.Generic;
using Common;
using Game.Hunting.HuntCamera;
using UnityEngine;

namespace Game.Hunting
{
    public class HuntPackSpawner : MonoBehaviour, IHuntPackSpawner
    {
        [SerializeField] private float _randomOffset;
        [SerializeField] private GameObject _packPrefab;
        [SerializeField] private RectGrid _rectGrid;
        [SerializeField] private CamFollower _camFollower;
        
        
        public IHunterPack SpawnPack()
        {
            // DebugAddGrid();
            var setup = GC.ActiveGridSO.GetSetup();
            var huntersRepo = GC.HuntersRepository;
            var rowsCount = setup.RowsCount;
            _rectGrid.SetCenterFront(setup.GetRow(0).CellsCount, setup.RowsCount, true);
            
            var packInstance = Instantiate(_packPrefab);
            packInstance.transform.SetPositionAndRotation(_rectGrid.center.position, _rectGrid.center.rotation);
            var pack = packInstance.GetComponent<IHunterPack>();
            
            var hunters = new List<IHunter>();
            for (var y = 0; y < rowsCount; y++)
            {
                var row = setup.GetRow(y);
                if (row.IsAvailable == false)
                    break;
                var count = row.CellsCount;
                for (var x = 0; x < count; x++)
                {
                    var item = row.GetCell(x).Item;
                    if (item != null)
                    {
                        var data = huntersRepo.GetHunter(item);
                        var instance = Instantiate(data.GetPrefab(), packInstance.transform);
                        var hunter = instance.GetComponent<IHunter>();
                        hunter.Init(data.GetSettings());
                        var localPos = _rectGrid.GetPositionXZ(x, y);
                        localPos.z += UnityEngine.Random.Range(-_randomOffset, _randomOffset);
                        var worldPos = _rectGrid.GetWorld(localPos);
                        instance.transform.SetPositionAndRotation( worldPos, packInstance.transform.rotation );
                        hunters.Add(hunter);
                    }
                }
            }
            pack.SetHunters(hunters);
            pack.SetCamera(_camFollower);
            return pack;
        }
        
        
    }
}