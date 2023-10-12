using Game.Merging;

namespace Game.UI.Merging
{
    public interface IActiveGroupSO
    {
        IActiveGroup Group();
        void SetGroup(IActiveGroup data);
    }
}