namespace Game.Merging
{
    public interface IHuntersRepository
    {
        IHunterData GetHunter(MergeItem item);
        int GetMaxLevel();
    }
}