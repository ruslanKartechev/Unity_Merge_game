using UnityEngine;
using System;
using Utils;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/"+nameof(SuperEgg), fileName = nameof(SuperEgg), order = -1)]
    public class SuperEgg : ScriptableObject
    {
        [SerializeField] private MergeItemSO _item;
        [SerializeField] private string _label;
        [SerializeField] private TimerTime _unclokDuration;
        [NonSerialized] private bool _isTicking;
        [NonSerialized] private TimerTime _timeLeft;
        
        public MergeItem Item => _item.Item;
        public string Label => _label;
        public bool IsTicking => _isTicking;

        public TimerTime BeginTime { get; set; }
        public TimerTime EndTime { get; set; }
        public TimerTime CurrentTime => new TimerTime(DateTime.Now);

        public void StopTicking()
        {
            _isTicking = false;
        }
        
        public TimerTime TimeLeft
        {
            get
            {
                _timeLeft = EndTime - CurrentTime;
                return _timeLeft;
            }
        }
        
        public void StartTicking()
        {
            BeginTime = new TimerTime(DateTime.Now);
            EndTime = BeginTime + _unclokDuration;
            _isTicking = true;
            CLog.LogWHeader("SuperEgg", 
            $"Unlocked super egg: {_label}. Start Time: {BeginTime.TimeAsString}, EndTime: {EndTime.TimeAsString}",
            "g", "w");
        }
        
        public void Init(bool isTicking, TimerTime beginTime, TimerTime endTime)
        {
            _isTicking = isTicking;
            BeginTime = beginTime;
            EndTime = endTime;
        }
        
        public void Init(SuperEggSaveData data)
        {
            _isTicking = data.IsTicking;
            BeginTime = data.BeginTime;
            EndTime = data.EndTime;
        }

        public SuperEggSaveData SaveData => new SuperEggSaveData()
        {
            IsTicking = _isTicking,
            BeginTime = BeginTime,
            EndTime = EndTime
        };
    }

    [System.Serializable]
    public class SuperEggSaveData
    {
        public bool IsTicking;
        public TimerTime BeginTime;
        public TimerTime EndTime;
    }
}