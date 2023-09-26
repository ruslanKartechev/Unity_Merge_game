using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class CarParticles : MonoBehaviour
    {
        [SerializeField] private List<ParticlesByEnvironment> _data;
        private List<GameObject> _particle = new List<GameObject>();

        
        public void Spawn()
        {
            foreach (var data in _data)
                _particle.Add(data.Spawn());   
        }

        public void Play()
        {
            foreach (var pp in _particle)
                pp.SetActive(true);
        }

        public void Hide()
        {
            foreach (var pp in _particle)
                pp.SetActive(false);
        }

        [System.Serializable]
        public class ParticlesByEnvironment
        {
            public Transform spawnPoint;
            public List<GameObject> particlesPrefabs;

            public GameObject Spawn()
            {
                var prefab = GetPrefab();
                var instance = Instantiate(prefab, spawnPoint);
                instance.transform.localPosition = Vector3.zero;
                instance.SetActive(false);
                return instance;
            }
            
            public GameObject GetPrefab()
            {
                var index = GC.PlayerData.EnvironmentIndex;
                return particlesPrefabs[index];
            }
        }
    }
}