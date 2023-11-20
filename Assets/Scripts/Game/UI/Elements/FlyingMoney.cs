using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Elements
{
    public class FlyingMoney : MonoBehaviour, IFlyingMoney
    {
        [SerializeField] private float _flyTime;
        [SerializeField] private float _scaleTime;
        [SerializeField] private List<FlyingMoneyElement> _flyingMoneyElements;
        private int _index;

        public void FlySingle()
        {
            CorrectIndex();
            var element = _flyingMoneyElements[_index];
            element.FlyTo( _scaleTime, _flyTime, UIC.Money.GetFlyToPosition());
        }

        public void FlySingle(Vector3 fromPosition)
        {
            CorrectIndex();
            fromPosition = Camera.main.WorldToScreenPoint(fromPosition);
            var element = _flyingMoneyElements[_index];
            element.FlyTo( _scaleTime, _flyTime, UIC.Money.GetFlyToPosition(), fromPosition);      
        }

        private void CorrectIndex()
        {
            if(_index >= _flyingMoneyElements.Count)
                _index = 0;
        }
    }
}