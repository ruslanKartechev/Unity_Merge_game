using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class BindingRopes : MonoBehaviour
    {
        [SerializeField] private List<Rope> _ropes;
        private Queue<float> _healthPoints;
        private int _ropeIndex = 0;
        
        
        private void Awake()
        {
            InitHealthPoints();
        }

        [ContextMenu("Calculate Health Points")]
        public void InitHealthPoints()
        {
            var count = _ropes.Count;
            _healthPoints = new Queue<float>(count);
            var step = 1f / count;
            for (var i = 0; i < count; i++)
            {
                _healthPoints.Enqueue((i + 1) * step);
                Debug.Log($"Step: {(i + 1) * step}");
            }
        }

        public void DropToHealth(float healthPercent)
        {
            if (_healthPoints.Count == 0)
                return;
            var point = 0f;
            do
            {
                point = _healthPoints.Peek();
                if (healthPercent <= point)
                {
                    _healthPoints.Dequeue();
                    _ropes[_ropeIndex].Drop();
                    _ropeIndex++;
                }
            } while (_healthPoints.Count > 0 && healthPercent <= point);
        }
    }
}