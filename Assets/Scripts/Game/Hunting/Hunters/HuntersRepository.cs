using Common.Data;
using Game.Merging;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    [CreateAssetMenu(menuName = "SO/" + nameof(HuntersRepository), fileName = nameof(HuntersRepository), order = 0)]
    public class HuntersRepository : ScriptableObject, IHuntersRepository
    {
        [SerializeField] private CollectionDataByID<HunterViewProvider> _huntersViews;
        
        public void Init()
        {
            _huntersViews.InitTable();
        }
        
        public IHunterViewProvider GetHunterData(string id)
        {
            return _huntersViews.GetObject(id);
        }
        
    }
}