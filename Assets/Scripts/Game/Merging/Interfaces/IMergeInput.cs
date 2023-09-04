using Game.UI.Merging;

namespace Game.Merging
{
    public interface IMergeInput
    {
        void Init(IMergingPage page, IMergeItemSpawner spawner);
        void Activate();
        void Stop();
    }
}