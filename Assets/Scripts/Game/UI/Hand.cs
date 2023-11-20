using System.Collections;
using UnityEngine;

namespace Game.UI
{
    [DefaultExecutionOrder(-200)]
    public class Hand : MonoBehaviour
    {
        private static Hand _instance;

        [SerializeField] private float _downScale;
        [SerializeField] private float _clickDownTime;
        [SerializeField] private float _clickUpTime;
        [SerializeField] private float _clickingDelay;
        [SerializeField] private Transform _scalable;
        [SerializeField] private Transform _movable;
        [Header("Releasing")] 
        [SerializeField] private float _releaseDownScale;
        [SerializeField] private float _releaseUpTime;
        [SerializeField] private float _releaseDownTime;
        [SerializeField] private float _releaseTopDelay;

        [Space(10)] 
        [SerializeField] private float _moveEndStayDelay;
        [SerializeField] private float _moveBackTimeFraction;
        [SerializeField] private AnimationCurve _moveCurve;

        private Coroutine _working;
        
        public static void ShowClickingAt(Vector3 position) => _instance.ClickAt(position);
        public static void ShowReleaseAt(Vector3 position) => _instance.ReleaseAt(position);
        public static void ShowRelease() => _instance.ReleaseAt(_instance._movable.position);
        
        
        public static void Hide(bool now = false) => _instance.HideM(now);

        public static void MoveFromTo(Vector3 from, Vector3 to, float time)
            => _instance.MoveFromToM(from, to, time);
        
        public void Awake()
        {
            if (_instance != null)
                Debug.Log($"Error! Two tutor hands");
            _instance = this;
            _movable.gameObject.SetActive(false);
        }

        private void ClickAt(Vector3 position)
        {
            StopWorking();
            _movable.gameObject.SetActive(true);
            _movable.transform.position = position;
            _working = StartCoroutine(Clicking());
        }

        private void ReleaseAt(Vector3 position)
        {
            StopWorking();
            _movable.gameObject.SetActive(true);
            _movable.transform.position = position;
            _working = StartCoroutine(Releasing());   
        }

        private void HideM(bool now = false)
        {
            StopWorking();
            if(now)
                Hide();
            else
                _working = StartCoroutine(Hiding());
        }

        private void MoveFromToM(Vector3 from, Vector3 to, float time)
        {
            StopWorking();
            _movable.gameObject.SetActive(true);
            _working = StartCoroutine(Moving(from, to, time));
        }

        private IEnumerator Moving(Vector3 from, Vector3 to, float time)
        {
            while (true)
            {
                _scalable.localScale = Vector3.one;
                _movable.position = from;
                yield return ScaleChange(1f, _downScale, _clickDownTime);
                yield return null;
                yield return null;
                
                var elapsed = 0f;
                while (elapsed <= time)
                {
                    var t = elapsed / time;
                    _movable.position = Vector3.Lerp(from, to, t);      
                    elapsed += Time.unscaledDeltaTime * _moveCurve.Evaluate(t);
                    yield return null;
                }
                _movable.position = to;
                
                yield return ScaleChange(_downScale, 1f, _clickUpTime);
                yield return new WaitForSecondsRealtime(_moveEndStayDelay);
                
                elapsed = 0f;
                var backTime = time * _moveBackTimeFraction;
                while (elapsed <= backTime)
                {
                    var t = elapsed / backTime;
                    _movable.position = Vector3.Lerp(to, from, t);      
                    elapsed += Time.unscaledDeltaTime * _moveCurve.Evaluate(t);
                    yield return null;
                }
                _movable.position = from;
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }        

        private IEnumerator Clicking()
        {
            while (true)
            {
                _scalable.localScale = Vector3.one;
                yield return ScaleChange(1f, _downScale, _clickDownTime);
                yield return new WaitForSecondsRealtime(_clickingDelay);
                yield return ScaleChange(_downScale, 1f, _clickDownTime * 0.3f);
            }            
        }

        private IEnumerator ScaleChange(float from, float to, float time)
        {
            var elapsed = 0f;
            while (elapsed <= time)
            {
                _scalable.localScale = Vector3.one * Mathf.Lerp(from, to, elapsed / time);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            _scalable.localScale = Vector3.one * to;
        }

        private IEnumerator Hiding()
        {
            yield return ScaleChange(1.1f, 0.25f, 0.2f);
            Hide();
        }

        
        
        private IEnumerator Releasing()
        {
            while (true)
            {
                _scalable.localScale = Vector3.one * _releaseDownScale;
                yield return ScaleChange(_releaseDownScale, 1f, _releaseUpTime);
                yield return new WaitForSecondsRealtime(_releaseTopDelay);
                yield return ScaleChange(1f, _releaseDownScale, _releaseDownTime);
            }            
        }
        
        private void Hide()
        {
            _movable.gameObject.SetActive(false);
        }

        private void StopWorking()
        {
            if(_working != null)
                StopCoroutine(_working);
        }
    }
}