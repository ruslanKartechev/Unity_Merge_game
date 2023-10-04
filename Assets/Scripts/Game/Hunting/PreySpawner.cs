using Dreamteck.Splines;
using UnityEngine;

namespace Game.Hunting
{
    // NOT USED ANYMORE
    public class PreySpawner : MonoBehaviour, IPreySpawner
    {
        [SerializeField] private Transform _spawnPoint;
        
        public IPreyPack Spawn(SplineComputer spline, ILevelSettings levelSettings)
        {
            var prefab = levelSettings.GetLevelPrefab();
            var instance = Instantiate(prefab);
            instance.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            var prey = instance.GetComponent<IPreyPack>();
            prey.Init(spline, levelSettings);
            return prey;
        }
        
    }
}