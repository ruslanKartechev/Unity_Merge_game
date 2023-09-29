using System;
using System.Collections;
using Common;
using UnityEngine;

namespace Game.Shop
{
    public class EggView : MonoBehaviour, IShopEgg
    {
        [SerializeField] private float _duration;
        [SerializeField] private EggCracker _cracker;
        [SerializeField] private GameObject _innerEgg;
        [SerializeField] private VerticalRotator _rotator;
        [Space(10)] 
        [SerializeField] private ParticleSystem _breakParticles;
        [SerializeField] private Animator _eggAnimator;
        [SerializeField] private string _eggBreakKey;
        [SerializeField] private float _eggAnimDuration;
        
        private Coroutine _working;
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_cracker == null)
                _cracker = gameObject.GetComponent<EggCracker>();
        }

        [ContextMenu("Test")]
        public void Test()
        {
            Crack(() => { Debug.Log("Test is over");});
        }
        #endif
        
        public void Crack(Action onEnd)
        {
            Stop();
            _working = StartCoroutine(Working(onEnd));
        }

        public void Stop()
        {
            if(_working != null)
                StopCoroutine(_working);
        }

        private IEnumerator Working(Action onEnd)
        {
            _cracker.Crack(_duration);
            while (_cracker.IsWorking)
                yield return null;
            _rotator.enabled = false;
            _innerEgg.gameObject.SetActive(false);
            _eggAnimator.Play(_eggBreakKey);
            if(_breakParticles != null)
                _breakParticles.Play();
            yield return new WaitForSeconds(_eggAnimDuration);
            onEnd.Invoke();
        }
        
    }
}