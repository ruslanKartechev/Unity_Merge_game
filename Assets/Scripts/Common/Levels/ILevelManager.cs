namespace Common.Levels
{
    public interface ILevelManager
    {
        void LoadCurrent();
        void LoadNext();
        void LoadPrev();
    }
}