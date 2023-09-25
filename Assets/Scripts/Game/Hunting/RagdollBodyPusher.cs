using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class RagdollBodyPusher : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> _parts;
        [SerializeField] private float _forwardForce;
        [SerializeField] private float _upForce;

        public void Push(Vector3 forwardDir)
        {
            var force = forwardDir * _forwardForce + Vector3.up * _upForce;
            StartCoroutine(Delayed(force));
        }

        private IEnumerator Delayed(Vector3 force)
        {
            yield return null;
            yield return new WaitForFixedUpdate();
            foreach (var rb in _parts)
                rb.AddForce(force, ForceMode.Impulse);   
        }
    }
}