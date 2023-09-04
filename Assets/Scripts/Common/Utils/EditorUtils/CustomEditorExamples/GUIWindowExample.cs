using EditorUtils.EditorWindows;
using UnityEngine;

namespace EditorUtils.CustomEditorExamples
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