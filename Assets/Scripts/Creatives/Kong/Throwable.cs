using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Creatives.Kong
{
    public class Throwable : MonoBehaviour
    {
        // -58.0.0
        // Quaternion(0.487596691,-1.75088587e-07,1.73458773e-07,0.873068988)
        //Pos  Vector3(-0.134000003,-0.105999999,-0.713)
        // eulers Vector3(58.3789864,21.7942924,23.6762791)
        [SerializeField] private float _rotTime = .2f;
        [SerializeField] private Vector3 _localRot = new Vector3(58.3789864f, 21.7942924f, 23.6762791f);
        [SerializeField] private Vector3 _localPos = new Vector3(-0.134000003f, -0.105999999f, -0.713f);
        public Rigidbody rb;
        
        public void Grab(Transform parent)
        {
            rb.isKinematic = true;
            transform.parent = parent;
            StartCoroutine(Rotating());
        }

        public void Throw(Vector3 force)
        {
            transform.parent = null;
            rb.isKinematic = false;
            rb.AddForce(force, ForceMode.VelocityChange);
            StopAllCoroutines();
        }

        private IEnumerator Rotating()
        {
            var time = _rotTime;
            var elapsed = 0f;
            var tr = transform;
            var rot1 = tr.localRotation;
            var rot2 = Quaternion.Euler(_localRot);
            var pos1 = tr.localPosition;
            var pos2 = _localPos;
            while (elapsed <= time)
            {
                var t = elapsed / time;
                tr.localRotation = Quaternion.Lerp(rot1, rot2, t);
                tr.localPosition = Vector3.Lerp(pos1, pos2, t);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}