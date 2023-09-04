namespace Game.Merging
{
    public interface IMergeGridRow
    {
        bool IsAvailable { get; set; }
        int CellsCount { get; }
        IGridCellData GetCell(int index);

    }
}