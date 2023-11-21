using System;
using Game.Merging;
using GC = Game.Core.GC;

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

        public void WinBoss(float award, SuperEgg egg, Action onContinue)
        {
            Darken();
            var ui = GC.UIManager.BossLevelWinPopup;
            ui.SetAward(award);
            ui.SetOnClicked(() =>
            {
                ui.Hide(false);
                onContinue.Invoke();
            });
            ui.Show(egg);            
        }
        
        public void Loose(float award, Action onReplay, Action onExit)
        {
            Darken();
            var ui = GC.UIManager.LoosePopup;
            ui.SetOnClicked(onExit, onReplay);
            ui.SetReward(award);
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