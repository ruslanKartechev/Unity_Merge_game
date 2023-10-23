﻿using System.Collections.Generic;
using Common;
using Dreamteck.Splines;
using Game.Hunting.Hunters.Interfaces;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting
{
    public class HuntPackSpawner : MonoBehaviour, IHuntPackSpawner
    {
        [SerializeField] private float _randomOffset;
        [SerializeField] private GameObject _packPrefab;
        [SerializeField] private RectGrid _rectGrid;
        private int _totalPower = 0;

        public IHunterPack SpawnPack(MovementTracks track)
        {
            var setup = GC.ActiveGroupSO.Group();
            var repository = GC.HuntersRepository;
            var rowsCount = setup.RowsCount;
            _rectGrid.SetCenterFront(setup.GetRow(0).CellsCount, setup.RowsCount, true);
            
            var packInstance = Instantiate(_packPrefab);
            packInstance.transform.SetPositionAndRotation(_rectGrid.center.position, _rectGrid.center.rotation);
            var pack = packInstance.GetComponent<IHunterPack>();
            var hunters = new List<IHunter>();
            
            var separateWater = track.water != null;
            var waterCount = 0;
            SplineSample sample = null;
            if (separateWater)
                sample = track.water.Project(_rectGrid.CenterWorldPoint);
            Debug.Log($"Separate water: {separateWater}");
            
            for (var y = 0; y < rowsCount; y++)
            {
                var row = setup.GetRow(y);
                if (row.IsAvailable == false)
                    break;
                var count = row.CellsCount;
                for (var x = 0; x < count; x++)
                {
                    var itemInd = count - x - 1;
                    var item = row.GetCell(itemInd).Item;
                    if (MergeItem.Empty(item))
                        continue;
                    var data = repository.GetHunterData(item.item_id);
                    var hunterGo = Instantiate(data.GetPrefab(), packInstance.transform);
                    var hunter = hunterGo.GetComponent<IHunter>();
                    if (item.class_id == MergeItem.WaterClass && separateWater)
                    {
                        var worldPos = sample.position - sample.right * waterCount * _rectGrid.xSpace;
                        hunterGo.transform.SetPositionAndRotation(worldPos, packInstance.transform.rotation);
                        hunters.Add(hunter);
                    }
                    else
                    {
                        var localPos = _rectGrid.GetPositionXZ(x, y);
                        localPos.z += UnityEngine.Random.Range(-_randomOffset, _randomOffset);
                        var worldPos = _rectGrid.GetWorld(localPos);
                        hunterGo.transform.SetPositionAndRotation(worldPos, packInstance.transform.rotation);
                        hunters.Add(hunter);
                    }
                    Debug.Log($"hunterGo {hunterGo.name}, ID {item.item_id}");
                    hunter.Init(item.item_id, track);
                }
            }
            pack.SetHunters(hunters);
            return pack;
        }
        
    }
    
    
}