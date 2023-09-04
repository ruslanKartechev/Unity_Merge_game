namespace Game.Hunting
{
    public interface IPreySettings
    {
        public float MoveSpeed { get; }
        public float Health { get; }
        public float Reward { get; }
        public float RotSpeed { get; }
    }
}