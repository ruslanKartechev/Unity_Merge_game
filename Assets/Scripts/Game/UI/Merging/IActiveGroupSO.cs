using Game.Merging;

namespace Game.UI.Merging
{
    public interface IActiveGroupSO
    {
        IActiveGroup Group();
        void SetSetup(IActiveGroup data);
    }
}