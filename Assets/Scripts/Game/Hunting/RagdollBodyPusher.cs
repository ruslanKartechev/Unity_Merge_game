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
            foreach (var rb in _parts)
            {
                rb.AddForce(force, ForceMode.Impulse);   
            }
        }

    }
}