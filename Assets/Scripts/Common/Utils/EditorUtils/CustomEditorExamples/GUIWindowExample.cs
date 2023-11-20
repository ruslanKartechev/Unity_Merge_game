using Common.Utils.EditorUtils.EditorWindows;
using UnityEngine;

namespace Common.Utils.EditorUtils.CustomEditorExamples
{
    
    public class GUIWindowExample : MonoBehaviour
    {
        public void Open()
        {
            #if UNITY_EDITOR
            CustomEditorWindow.ShowMyEditor();
            #endif
        }
        
        public void Close()
        {
                
        }
        
    }
}