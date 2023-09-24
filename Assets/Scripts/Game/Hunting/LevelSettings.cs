using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(LevelSettings), fileName = nameof(LevelSettings), order = 0)]
    public class LevelSettings :  ScriptableObject, ILevelSettings
    {
        [SerializeField] private GameObject _preyPackPrefab;
        [SerializeField] private int _cameraFlyDir = 1;

        public int CameraFlyDir => _cameraFlyDir;
        public GameObject GetPreyPack()
        {
            return _preyPackPrefab;
        }
    }
}