namespace Game.Merging
{
    public interface IHuntersRepository
    {
        IHunterViewProvider GetHunterData(string itemId);
    }
}