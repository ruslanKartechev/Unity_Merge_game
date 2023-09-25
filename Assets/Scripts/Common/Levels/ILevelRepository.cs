namespace Common.Levels
{
    public interface ILevelRepository
    {
        public EnvironmentLevel GetEnvironment(int index);
        public int Count { get; }
    }
}