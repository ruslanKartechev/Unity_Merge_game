using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Creatives
{
    public class ZAxisAnimator : MonoBehaviour
    {
        [SerializeField] private bool _autoStart;
        [SerializeField] private Transform _target;
        [SerializeField] private List<CurveData> _options;

        private Coroutine _working;

        private void Start()
        {
            if (_autoStart)
                Begin();
        }

        public void Begin()
        {
            Stop();
            _working = StartCoroutine(Working());         
        }

        public void Stop()
        {
            if (_working != null)
                StopCoroutine(_working);   
        }

        private IEnumerator Cycle()
        {
            var option = _options.Random();
            var curve = option.curve;
            var elapsed = 0f;
            var time = option.time;
            var t = 0f;
            var startY = _target.localPosition.z;
            var magn = option.magnitude;
            while (t <= 1)
            {
                var val = startY + magn * curve.Evaluate(t);
                SetVal(val);
                t = elapsed / time;
                elapsed += Time.deltaTime;
                yield return null;
            }
            SetVal(startY + magn * curve.Evaluate(1));

            void SetVal(float val)
            {
                var pos = _target.localPosition;
                pos.z = val;
                _target.localPosition = pos;
            }
        }

        private IEnumerator Working()
        {
            while (true)
            {
                yield return Cycle();
            }
        }
    }
}