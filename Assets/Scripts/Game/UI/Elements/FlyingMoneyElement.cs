using DG.Tweening;
using UnityEngine;

namespace Game.UI.Elements
{
    public class FlyingMoneyElement : MonoBehaviour
    {
        private Vector3 _startPos;

        
        public void FlyTo(float scaleTime, float flyTime, Vector3 flyTo, Vector3 flyFrom, float amount)
        {
            var tr = transform;
            tr.position = _startPos = flyFrom;
            gameObject.SetActive(true);
            tr.localScale = Vector3.zero;
            tr.DOScale(Vector3.one, scaleTime);
            tr.DOMove(flyTo, flyTime).SetDelay(scaleTime).SetEase(Ease.InQuad).OnComplete(() =>
            {
                gameObject.SetActive(false);
                tr.position = _startPos;
                // UIC.Money.UpdateCount(true);
                var money = (MoneyUI)UIC.Money;
                money?.UpdateCount(amount, true);
            });
            
        }
        
        public void FlyTo(float scaleTime, float flyTime, Vector3 flyTo, Vector3 flyFrom)
        {
            var tr = transform;
            tr.position = _startPos = flyFrom;
            gameObject.SetActive(true);
            tr.localScale = Vector3.zero;
            tr.DOScale(Vector3.one, scaleTime);
            tr.DOMove(flyTo, flyTime).SetDelay(scaleTime).OnComplete(() =>
            {
                gameObject.SetActive(false);
                tr.position = _startPos;
                UIC.Money.UpdateCount(true);
            });
        }

        public void FlyTo(float scaleTime, float flyTime, Vector3 flyTo)
        {
            var tr = transform;
            _startPos = tr.position;
            gameObject.SetActive(true);
            tr.localScale = Vector3.zero;
            tr.DOScale(Vector3.one, scaleTime);
            tr.DOMove(flyTo, flyTime).SetDelay(scaleTime).OnComplete(() =>
            {
                tr.position = _startPos;
                gameObject.SetActive(false);
                UIC.Money.UpdateCount(true);
            });
            
        }
        
    }
}