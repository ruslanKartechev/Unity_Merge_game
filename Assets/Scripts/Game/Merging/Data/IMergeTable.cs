namespace Game.Merging
{
    public interface IMergeTable
    {
        public MergeItem GetItem(int level, string classId);
        public MergeItem GetItem(MergeItem item1, MergeItem item2);
        
    }
}