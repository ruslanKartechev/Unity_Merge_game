using Dreamteck.Splines;
using UnityEngine;

namespace Game.Hunting
{
    public class PreySpawner : MonoBehaviour, IPreySpawner
    {
        [SerializeField] private Transform _spawnPoint;
        
        public IPreyPack Spawn(SplineComputer spline, ILevelSettings levelSettings)
        {
            var prefab = levelSettings.GetPreyPack();
            var instance = Instantiate(prefab);
            instance.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            var prey = instance.GetComponent<IPreyPack>();
            prey.Init(spline);
            return prey;
        }
        
    }
}