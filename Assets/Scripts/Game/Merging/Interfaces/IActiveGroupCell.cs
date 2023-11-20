namespace Game.Merging.Interfaces
{
    public interface IActiveGroupCell
    {
        public bool Purchased { get; set; }
        public float Cost { get; set; }
        public MergeItem Item { get; set; }   
    }
}