#if UNITY_EDITOR
using Common.Utils.EditorUtils;
using UnityEditor;
using UnityEngine;

namespace Common.UILayout
{
    [CustomEditor(typeof(LayoutSwitcher))]
    public class LayoutSwitcherEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as LayoutSwitcher;
            GUILayout.BeginHorizontal();
            if (EU.ButtonBig("Prev", Color.cyan))
                me.SetPrevLayout();
            if (EU.ButtonBig("Next", Color.cyan))
                me.SetNextLayout();   
            GUILayout.EndHorizontal();
        }
    }
}
#endif