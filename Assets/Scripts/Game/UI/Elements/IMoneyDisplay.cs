namespace Game.UI.Elements
{
    public interface IMoneyDisplay
    {

        void UpdateCount(bool animated = true);
        void Highlight();
    }
}