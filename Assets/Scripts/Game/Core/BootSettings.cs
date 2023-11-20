using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "SO/" + nameof(BootSettings), fileName = nameof(BootSettings), order = 0)]
    public class BootSettings : ScriptableObject
    {
        public bool CapFPS;
        public int FpsCap = 60;
        [Space(10)]
        public bool InitAnalytics;
        public bool ShowTerms;
        public bool ShowPregameCheat;
        [Space(10)] 
        public bool RunResolutionScaler;
        [Space(10)]
        public bool ClearAllSaves;
        public bool UseDebugConsole;
        public bool UseDevUI;
        [Space(10)] 
        public bool doPeriodicSave = true;
        public float dataSavePeriod = 10;
    }
}