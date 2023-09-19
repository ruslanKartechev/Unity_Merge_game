using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Hunting
{
    public interface IPreyDamageEffect
    {
        public void Play();
    }
    public class PreyDamageEffect : MonoBehaviour, IPreyDamageEffect
    {
        [SerializeField] private Transform _scalable;
        [SerializeField] private float _scaleMagn;
        [SerializeField] private float _scaleTime;
        [SerializeField] private Ease _scaleEase;
        [Space(10)] 
        [SerializeField] private float _materialSwitchTime;
        [SerializeField] private List<Material> _damagedMat;
        [SerializeField] private Renderer _renderer;
        
        
        public void Play()
        {
            _scalable.localScale = Vector3.one * (1 + _scaleMagn);
            _scalable.DOScale(Vector3.one, _scaleTime).SetEase(_scaleEase);
            StartCoroutine(MaterialSwitch());
        }

        private IEnumerator MaterialSwitch()
        {
            var oldMat = _renderer.sharedMaterials;
            _renderer.sharedMaterials = _damagedMat.ToArray();
            yield return new WaitForSeconds(_materialSwitchTime);
            _renderer.sharedMaterials = oldMat;
        }
    }
}