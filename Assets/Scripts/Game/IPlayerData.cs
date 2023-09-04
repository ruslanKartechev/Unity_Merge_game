using System;

namespace Game
{
    public interface IPlayerData
    {
        /// <summary>
        /// Notifies on money count updated. (OldCount, NewCount)
        /// </summary>
        public event Action<float, float> OnMoneyUpdated; 
        float Money{ get; set; }
        int LevelIndex{ get; set; }
        int LevelTotal { get; set; }
    }
}