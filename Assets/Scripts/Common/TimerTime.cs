namespace Game.Merging
{
    [System.Serializable]
    public struct TimerTime
    {
        public int Hour;
        public int Minute;
        public int Second;

        public static TimerTime Empty => new TimerTime(-1, -1, -1);
        public static TimerTime Zero => new TimerTime(0, 0, 0);

        public void CorrectToZero()
        {
            if(Second < 0)
                Second = 0;
            if(Minute < 0)
                Minute = 0;
            if(Hour < 0)
                Hour = 0;
        }
        
        public string TimeAsString
        {
            get
            {
                var result = $"{Hour:00}:{Minute:00}:{Second:00}";
                return result;
            }
        }

        public TimerTime(int hour, int minute, int second)
        {
            Hour = hour;
            if(minute >= 0)
                Minute = minute;
            else
            {
                Hour--;
                Minute = 60 + minute;
            }
            if(second >= 0)
                Second = second;
            else
            {
                Minute--;
                Second = (60 + second);
            }
        }

        public TimerTime(System.DateTime dateTime)
        {
            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;
        }

        public static TimerTime operator +(TimerTime a, TimerTime b)
        {
            return new TimerTime(a.Hour + b.Hour, a.Minute + b.Minute, a.Second + b.Second);
        }
        
        public static TimerTime operator -(TimerTime a, TimerTime b)
        {
            return new TimerTime(a.Hour - b.Hour, a.Minute - b.Minute, a.Second - b.Second);
        }
        
        
        public static bool operator ==(TimerTime a, TimerTime b)
        {
            return (a.Hour == b.Hour) && (a.Minute == b.Minute) && (a.Second == b.Second);
        }
        
        public static bool operator !=(TimerTime a, TimerTime b)
        {
            return !(a == b);
        }
        
     
    }
}