namespace Game.Merging
{
    public interface IHuntersRepository
    {
        IHunterData GetItemByLevel(int level);
        int GetMaxLevel();
    }
}