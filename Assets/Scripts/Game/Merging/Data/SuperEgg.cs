using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/"+nameof(SuperEgg), fileName = nameof(SuperEgg), order = -1)]
    public class SuperEgg : ScriptableObject
    {
        [SerializeField] private MergeInput _item;
        [SerializeField] private string _label;
        [SerializeField] private float _unlockTime;
        
        public float TimerStartTime { get; set; }
        public float TimeLeft { get; set; }

        public string TimeLeftString
        {
            get
            {
                var span = System.TimeSpan.FromSeconds(TimeLeft);
                var result = $"{span.Hours}:{span.Minutes}:{span.Seconds}";
                return result;
            }
        }
        
        public void BeginTimer()
        {
            TimerStartTime = Time.time;   
            TimeLeft = _unlockTime;
        }
        
        public MergeInput Item => _item;
        public string Label => _label;
    }
}