using System;
using System.Collections;
using UnityEngine;

namespace Game.Shop
{
    public class EggView : MonoBehaviour, IShopEgg
    {
        [SerializeField] private float _duration;
        [SerializeField] private EggCracker _cracker;
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
            yield return new WaitForSeconds(_duration * 1.05f);
            onEnd.Invoke();
        }
        
    }
}