namespace Game.Hunting
{
    public interface ILevelSettings
    {
        IPreySettings PreySettings { get; }
        public IPreyData PreyData { get; }
    }
}