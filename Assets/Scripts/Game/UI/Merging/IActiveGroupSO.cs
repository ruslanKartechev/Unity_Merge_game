using Game.Merging;
using Game.Merging.Interfaces;

namespace Game.UI.Merging
{
    public interface IActiveGroupSO
    {
        IActiveGroup Group();
        void SetGroup(IActiveGroup data);
    }
}