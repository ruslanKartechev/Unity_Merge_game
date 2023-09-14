using Game.Merging;

namespace Game.UI.Merging
{
    public interface IMergeGridRepository
    {
        IActiveGroup GetSetup();
        void SetSetup(IActiveGroup data);
    }
}