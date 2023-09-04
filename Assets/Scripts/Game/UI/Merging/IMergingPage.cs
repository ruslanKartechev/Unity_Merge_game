namespace Game.UI.Merging
{
    public interface IMergingPage
    {
        void UpdateMoney();
        void SetPurchaseCost(float cost);
        void ShowPlayButton(bool animated);
        void HidePlayButton();
    }
}