using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.UIPop
{
    public class PopAnimator : MonoBehaviour
    {
        [SerializeField] private List<PopElement> _elements;

        private bool _isDone;
        public bool IsDone => _isDone;

        public void HideAll()
        {
            foreach (var el in _elements)
            {
                el.transform.localScale = Vector3.zero;
            }
        }

        public void HideAndPlay()
        {
            HideAll();
            StartCoroutine(Working());
        }
        
        public IEnumerator Working()
        {
            _isDone = false;
            var totalTime = 0f;
            foreach (var pop in _elements)
                totalTime += pop.Delay;
            var lastDur =_elements[^1].Duration;
            
            foreach (var pop in _elements)
            {
                pop.ScaleUp();
                yield return new WaitForSeconds(pop.Delay);
            }
            yield return new WaitForSeconds(lastDur);
            _isDone = true;
        }

    }
}