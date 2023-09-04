using System;

namespace Common.Scenes
{
    public interface ISceneSwitcher
    {
        void OpenSceneAdditive(string name, Action<bool> onLoaded);
        void OpenScene(string name, Action<bool> onLoaded);
        
        void ClosePrevAdditiveScene();
        void CloseAllPrevAdditiveScene();
        
    }
}