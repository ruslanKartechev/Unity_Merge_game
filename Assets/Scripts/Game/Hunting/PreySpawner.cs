using Dreamteck.Splines;
using UnityEngine;

namespace Game.Hunting
{
    public class PreySpawner : MonoBehaviour, IPreySpawner
    {
        [SerializeField] private Transform _spawnPoint;
        
        public IPrey Spawn(SplineComputer spline, ILevelSettings levelSettings)
        {
            var prefab = levelSettings.PreyData.Prefab;
            var instance = Instantiate(prefab);
            instance.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            var prey = instance.GetComponent<IPrey>();
            var settings = levelSettings.PreySettings;
            prey.Init(settings, spline);
            return prey;
        }
        
    }
}