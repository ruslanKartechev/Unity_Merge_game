namespace Game.Merging
{
    public interface IMergeTable
    {
        public MergeItem GetItem(int level, string classId);
        public MergeItem GetMergedItem(MergeItem item1, MergeItem item2);

    }
}