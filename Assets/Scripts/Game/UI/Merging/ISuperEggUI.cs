using Game.Merging;

namespace Game.UI.Merging
{
    public interface ISuperEggUI
    {
        void Show(SuperEgg egg);
        void MoveDown();
        void Hide();
        void ShowLabel();
        void HideLabel();
    }
}