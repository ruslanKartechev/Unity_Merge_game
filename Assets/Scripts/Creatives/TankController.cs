using System.Collections;
using UnityEngine;

namespace Creatives
{
    public class TankController : MonoBehaviour
    {
        [SerializeField] private TankRotator _tankRotator;
        [SerializeField] private Transform _p1;
        [SerializeField] private float _time1;
        [SerializeField] private float _delay1;
        [SerializeField] private Transform _p2;
        [SerializeField] private float _time2;
        [SerializeField] private float _delay2;
        [SerializeField] private Transform _p3;
        [SerializeField] private float _time3;
        [SerializeField] private float _delay3;

        private void Start()
        {
            StartCoroutine(Working());
        }

        private IEnumerator Working()
        {
            yield return new WaitForSeconds(_delay1);
            _tankRotator.RotateToPoint(_p1, _time1);
            yield return new WaitForSeconds(_delay2);
            _tankRotator.RotateToPoint(_p2, _time2);
            yield return new WaitForSeconds(_delay3);
            _tankRotator.RotateToPoint(_p3, _time3);
        }
    }
}