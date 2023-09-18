using Game.UI.Elements;

namespace Game.UI
{
    public static class UIC
    {
        
        public static IMoneyUI Money { get; set; }
        public static IMoneyUI Crystals { get; set; }

        
        public static void UpdateMoney()
        {
            Money.UpdateCount();
        }
        
        public static void UpdateCrystals()
        {
            Crystals.UpdateCount();
        }

        public static void UpdateMoneyAndCrystals()
        {
            UpdateMoney();
            UpdateCrystals();
        }

    }
}