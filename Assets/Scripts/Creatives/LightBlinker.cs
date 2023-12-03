using System.Collections;
using UnityEngine;

namespace Creatives
{
    public class LightBlinker : MonoBehaviour
    {
        [SerializeField] private float _randomDelayMin;
        [SerializeField] private float _randomDelayMax;        
        [SerializeField] private bool _doStart;
        [SerializeField] private Light _light;
        [SerializeField] private float _min;
        [SerializeField] private float _max;
        [SerializeField] private float _timeUp;
        [SerializeField] private float _timeDown;

        private void Start()
        {
            if(_doStart)
                Begin();
        }

        public void Begin()
        {
            StartCoroutine(Working());
        }

        private IEnumerator Working()
        {
            var delay = UnityEngine.Random.Range(_randomDelayMin, _randomDelayMax);
            if(delay > 0)
                yield return new WaitForSeconds(delay);
            while (true)
            {
                var elapsed = 0f;
                var time = _timeUp;
                while (elapsed < time)
                {
                    _light.intensity = Mathf.Lerp(_min, _max, elapsed / time);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                elapsed = 0f;
                time = _timeDown;
                while (elapsed < time)
                {
                    _light.intensity = Mathf.Lerp(_max, _min, elapsed / time);
                    elapsed += Time.deltaTime;
                    yield return null;
                }          
            }
      
        }
    }
}