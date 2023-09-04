namespace Game.Merging
{
    public interface IGridCellData
    {
        public bool Purchased { get; set; }
        public float Cost { get; set; }
        public int SpawnItemLevel { get; set; }   
    }
}