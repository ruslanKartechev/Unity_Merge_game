using System;
using System.Collections;
using Common;
using UnityEngine;

namespace Game.Merging
{
    public abstract class Tutorial : MonoBehaviour
    {
        [SerializeField] protected int _waitFramesCount = 5;
        [SerializeField] protected GameObject _tutorBlock;
        [SerializeField] protected TutorSpotlightUI _spotlight1;
        [SerializeField] protected TutorSpotlightUI _spotlight2;
        protected Action _onCompleted;
        protected Coroutine _delayedAction;

        public abstract void BeginTutorial(Action onCompleted);
        
        protected IEnumerator Delayed(float delay, Action onEnd)
        {
            yield return new WaitForSeconds(delay);
            yield return null;
            onEnd.Invoke();
        }
        
        protected IEnumerator SkipFrames(int frames, Action onEnd)
        {
            for(var i = 0; i < frames; i++)
                yield return null;
            onEnd.Invoke();
        }
    }
}