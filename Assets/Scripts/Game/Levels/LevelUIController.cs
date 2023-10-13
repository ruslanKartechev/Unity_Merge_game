using System;

namespace Game.Levels
{
    public class LevelUIController
    {
        public void Darken()
        {
            var ui = GC.UIManager.Darkening;
            ui.Show();
        }
        
        public void Win(float award, Action onContinue)
        {
            Darken();
            var ui = GC.UIManager.WinPopup;
            ui.SetAward(award);
            ui.SetOnClicked(() =>
            {
                ui.Hide(false);
                onContinue.Invoke();
            });
            ui.Show();
        }
        
        public void Loose(Action onReplay, Action onExit)
        {
            Darken();
            var ui = GC.UIManager.LoosePopup;
            ui.SetOnClicked(onExit, onReplay);
            ui.Show();
        }

        public void MapToNext(int level)
        {
            Darken();
            var ui = GC.UIManager.WinLevelMap;
            ui.MoveToLevel(level);
        }
        
    }
}