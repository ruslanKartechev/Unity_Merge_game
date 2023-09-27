using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(LevelEnvironment), fileName = nameof(LevelEnvironment), order = 0)]
    public class LevelEnvironment : ScriptableObject
    {
        [SerializeField] private string _sceneName;
        public string SceneName => _sceneName;
    }
}