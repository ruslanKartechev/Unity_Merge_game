using System.Collections.Generic;
using Game.Hunting;
using UnityEngine;

namespace Common.Levels
{
    [CreateAssetMenu(menuName = "SO/" + nameof(LevelsRepository), fileName = nameof(LevelsRepository), order = 0)]
    public class LevelsRepository : ScriptableObject, ILevelRepository 
    {
        [SerializeField] private List<EnvironmentLevel> _environments;
        
        public int Count => _environments.Count;
        
        public EnvironmentLevel GetEnvironment(int index)
        {
            if (index >= _environments.Count)
                index = 0;
            return _environments[index];
        }
    }
}