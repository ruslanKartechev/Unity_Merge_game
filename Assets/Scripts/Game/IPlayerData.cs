
namespace Game
{
    public interface IPlayerData
    {
        /// <summary>
        /// Notifies on money count updated. (OldCount, NewCount)
        /// </summary>
        float Money{ get; set; }
        float Crystal{ get; set; }
        int LevelTotal { get; set; }
        
        int CurrentEnvironmentIndex { get; set; }
        int ShopPurchaseCount { get; set; }
        
        
        bool TutorPlayed_Attack { get; set; }
        bool TutorPlayed_Merge { get; set; }
        bool TutorPlayed_Purchased { get; set; }
        
        
    }
}