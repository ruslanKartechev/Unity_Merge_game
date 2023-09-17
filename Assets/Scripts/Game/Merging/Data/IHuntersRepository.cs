namespace Game.Merging
{
    public interface IHuntersRepository
    {
        IHunterData GetHunterData(string itemId);
        int GetMaxLevel();
    }
}