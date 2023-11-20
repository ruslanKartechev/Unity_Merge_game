namespace Game.Merging.Interfaces
{
    public interface IActiveGroupRow
    {
        bool IsAvailable { get; set; }
        int CellsCount { get; }
        IActiveGroupCell GetCell(int index);

    }
}