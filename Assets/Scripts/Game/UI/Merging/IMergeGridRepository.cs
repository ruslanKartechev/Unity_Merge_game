using Game.Merging;

namespace Game.UI.Merging
{
    public interface IMergeGridRepository
    {
        IMergeGridData GetSetup();
        void SetSetup(IMergeGridData data);
    }
}