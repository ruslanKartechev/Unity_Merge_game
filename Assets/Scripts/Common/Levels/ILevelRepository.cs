using Game.Hunting;
namespace Common.Levels
{
    public interface ILevelRepository
    {
        // public EnvironmentLevel GetEnvironment(int index);
        ILevelSettings GetLevel(int index);
        public int Count { get; }
    }
}