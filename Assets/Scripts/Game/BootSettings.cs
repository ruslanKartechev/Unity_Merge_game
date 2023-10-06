﻿using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/" + nameof(BootSettings), fileName = nameof(BootSettings), order = 0)]
    public class BootSettings : ScriptableObject
    {
        public bool CapFPS;
        public int FpsCap = 60;
        [Space(10)]
        public bool ShowPregameCheat;
        public bool ClearAllSaves;
        public bool UseDebugConsole;
        public bool UseDevUI;
        [Space(10)] 
        public bool doPeriodicSave = true;
        public float dataSavePeriod = 10;
    }
}