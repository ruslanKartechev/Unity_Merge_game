namespace Game.Merging
{
    public interface IActiveGroupRow
    {
        bool IsAvailable { get; set; }
        int CellsCount { get; }
        IActiveGroupCell GetCell(int index);

    }
}