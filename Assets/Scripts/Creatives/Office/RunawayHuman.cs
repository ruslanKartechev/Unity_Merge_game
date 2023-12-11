using System;
using System.Collections;
using Dreamteck.Splines;
using UnityEngine;
using System.Collections;

namespace Creatives.Office
{
    public class RunawayHuman : MonoBehaviour
    {
        public bool autoStart = true;
        public HumanAnim runAnim;
        public OfficeHuman human;
        public SplineFollower splineFollower;
        public bool moveToEndPoint;
        public Transform endPoint;
        public Transform enterPoint;
        public float moveToEnterSpeed;
        public GameObject trigger;
        public float triggerCallPercent = .5f;
        [Space(10)] 
        [Header("Speedup")] 
        public bool useAcceleration;
        public float endSpeed;
        public float speedUpTime;
        public float speedUpDelay;
        private Coroutine _moving;
        private Coroutine _accelerating;
        
        [Space(10)]
        [SerializeField] private float _moneyFlyDelay;
        [SerializeField] private Transform _moneyPoint;
        [SerializeField] private bool _useFlyMoney = true;
        [SerializeField] private float _moneyReward = 100f;

        private void Start()
        {
            human.OnDead += OnDead;
            if (!autoStart)
                return;
            BeginRun();
        }

        public void BeginRun()
        {
            SplineHelper.SetOffset(splineFollower);
            splineFollower.enabled = true;
            splineFollower.follow = true;
            runAnim.Play(human.animator);
            if (moveToEndPoint)
            {
                StartCoroutine(CheckingEndPoint());
            }
            if (useAcceleration)
            {
                StopAccelerate();
                _accelerating = StartCoroutine(Accelerating());
            }            
        }

        private void OnDead()
        {
            splineFollower.enabled = false;
            human.DollDie();
            FlyMoney();
        }

        private void StopAccelerate()
        {
            if(_accelerating != null)
                StopCoroutine(_accelerating);
        }

        private IEnumerator Accelerating()
        {
            yield return new WaitForSeconds(speedUpDelay);
            var elapsed = 0f;
            var time = speedUpTime;
            var startSpeed = splineFollower.followSpeed;
            var endSpeed = this.endSpeed;
            while (elapsed <= time)
            {
                var speed = Mathf.Lerp(startSpeed, endSpeed, elapsed / time);
                splineFollower.followSpeed = speed;
                elapsed += Time.deltaTime;
                yield return null;
            }
        }        
        private IEnumerator CheckingEndPoint()
        {
            var res = new SplineSample();
            splineFollower.Project(endPoint.position, res);
            var threshold = .5f / 100f;
            var targetPercent = (float)res.percent;
            while (true)
            {
                var percent = (float)splineFollower.result.percent;
                var diff = Mathf.Abs(percent - targetPercent);
                // Debug.Log($"Diff {diff * 100f}, threshold: {threshold * 100f}");
                if (diff < threshold || percent > targetPercent)
                {
                    MoveToEndPoint();
                    yield break;
                }
                yield return null;
            }
        }

        private void MoveToEndPoint()
        {
            splineFollower.follow = false;
            StopAccelerate();
            StopMoving(); 
            _moving = StartCoroutine(MovingToPoint(enterPoint, moveToEnterSpeed, CallTrigger));
        }

        private void CallTrigger()
        {
            var trg = trigger.GetComponent<IHumanTrigger>();
            trg?.OnEntered();
        }

        private void StopMoving()
        {
            if(_moving != null)
                StopCoroutine(_moving);
        }
        
        private IEnumerator MovingToPoint(Transform point, float speed, Action onTrigger)
        {
            var rotSpeed = .33f;
            var elapsed = 0f;
            var t = 0f;
            var tr = transform;
            var startpos = tr.position;
            var time = (tr.position - point.position).magnitude / speed;
            var rot2 = Quaternion.LookRotation(point.position - startpos);
            var triggerred = false;
            while (t <= 1f)
            {
                tr.position = Vector3.Lerp(startpos, point.position, t);
                tr.rotation = Quaternion.Lerp(tr.rotation, rot2, t);
                t = elapsed / time;
                elapsed += Time.deltaTime;
                if (!triggerred)
                {
                    if (t >= triggerCallPercent)
                    {
                        onTrigger.Invoke();
                        triggerred = true;
                    }
                }
                yield return null;
            }
            tr.SetPositionAndRotation(point.position, rot2);
            
        }   
        
        
        private void FlyMoney()
        {
            if (_moneyFlyDelay <= 0)
            {
                CallMoney();
                return;
            }

            StartCoroutine(DelayedMoney(_moneyFlyDelay));

        }

        private void CallMoney()
        {
            if (_useFlyMoney && _moneyPoint != null)
            {
                var creos = CreosUI.Instance;
                if (creos == null)
                    return;
                creos.FlyingMoney.FlySingle(_moneyPoint.position, _moneyReward);
            }    
        }
        
        private IEnumerator DelayedMoney(float time)
        {
            yield return new WaitForSeconds(time);
            CallMoney();
        }
    }
}