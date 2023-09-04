namespace Game.Merging
{
    public interface IMergeGridData
    {
        int RowsCount { get; }
        IMergeGridRow GetRow(int index);
    }
}