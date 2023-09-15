using Game.Merging;

namespace Game.UI.Merging
{
    public interface IActiveGroupSO
    {
        IActiveGroup GetSetup();
        void SetSetup(IActiveGroup data);
    }
}