using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(LevelSettings), fileName = nameof(LevelSettings), order = 0)]
    public class LevelSettings :  ScriptableObject, ILevelSettings
    {
        [SerializeField] private PreySettings _preySettings;
        [SerializeField] private PreyData _preyData;
        
        public IPreySettings PreySettings => _preySettings;
        public IPreyData PreyData => _preyData;
    }
}