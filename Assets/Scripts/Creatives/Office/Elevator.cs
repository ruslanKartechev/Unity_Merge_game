using System.Collections;
using UnityEngine;

namespace Creatives.Office
{
    public class Elevator : MonoBehaviour, IElevator
    {
        public Door leftDoor;
        public Door rightDoor;
        public float closeTime = .5f;
        public AnimationCurve closeCurve;
        public AnimationCurve openCurve;
        
        [System.Serializable]
        public class Door
        {
            public Transform tr;
            public Vector3 localOpen;
            public Vector3 localShut;

            public void SetOpen(float t)
            {
                tr.localPosition = Vector3.Lerp(localShut, localOpen, t);
            }
            public void SetClose(float t)
            {
                tr.localPosition = Vector3.Lerp(localOpen, localShut, t);
            }
        }
        
        
        public void Close()
        {
            StartCoroutine(Closing());
        }

        public void Open()
        {
            StartCoroutine(Opening());
        }

        [ContextMenu("Set opened")]
        public void SetOpen()
        {
            leftDoor.SetOpen(1);
            rightDoor.SetOpen(1);
        }
        
        [ContextMenu("Set closed")]
        public void SetClose()
        {
            leftDoor.SetClose(1);
            rightDoor.SetClose(1);
        }
        

        private IEnumerator Closing()
        {
            var elapsed = 0f;
            var time = closeTime;
            var t = 0f;
            while (t <= 1f)
            {
                leftDoor.SetClose(t);
                rightDoor.SetClose(t);
                t = elapsed / time;
                elapsed += Time.deltaTime * closeCurve.Evaluate(t);
                yield return null;
            }
            leftDoor.SetClose(1);
            rightDoor.SetClose(1);
        }
        
        
        private IEnumerator Opening()
        {
            var elapsed = 0f;
            var time = closeTime;
            var t = 0f;
            while (t <= 1f)
            {
                leftDoor.SetOpen(t);
                rightDoor.SetOpen(t);
                t = elapsed / time;
                elapsed += Time.deltaTime * openCurve.Evaluate(t);
                yield return null;
            }
            leftDoor.SetOpen(1);
            rightDoor.SetOpen(1);
        }
    }
}