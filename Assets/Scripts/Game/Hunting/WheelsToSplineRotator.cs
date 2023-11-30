using System.Collections;
using System.Collections.Generic;
using Common.Utils;
using Dreamteck.Splines;
using UnityEngine;

namespace Game.Hunting
{
    public class WheelsToSplineRotator : MonoBehaviour
    {
        [SerializeField] private float _offsetPercent = 1.5f;
        [SerializeField] private SplineFollower _follower;
        [SerializeField] private Transform _tr;
        [SerializeField] private List<Transform> _targets;

        private void Start()
        {
            StartCoroutine(DelayedStart());   
        }

        private IEnumerator DelayedStart()
        {
            yield return null;
            yield return null;
            StartCoroutine(Rotating());           
        }

        private IEnumerator Rotating()
        {
            if (_follower == null)
            {
                CLog.LogRed("Follower == null");
                yield break;
            }

            if (_follower.spline == null)
            {
                CLog.LogRed("Spline not assigned yet");
                yield break;
            }
            var forwardPercentOffset = _offsetPercent / 100f;
            while (true)
            {
                var res1 = _follower.result;
                _follower.Project(_tr.position, res1);
                // Debug.Log($"Current %: {res1.percent}");
                var res2 = _follower.Evaluate(res1.percent + forwardPercentOffset);
                var lookVec = res2.position - res1.position;
                foreach (var target in _targets)
                {
                    target.rotation = Quaternion.LookRotation(lookVec);
                }
                yield return null;
            }
        }
    }
}