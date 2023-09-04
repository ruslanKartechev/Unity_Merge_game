using Game.Hunting;

namespace Common.Levels
{
    public interface ILevelRepository
    {
        public string GetLevelSceneName(int index);
        public ILevelSettings GetLevelSettings(int index);
    }
}