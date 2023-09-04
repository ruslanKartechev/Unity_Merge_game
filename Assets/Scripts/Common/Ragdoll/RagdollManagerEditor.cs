#if UNITY_EDITOR
using EditorUtils;
using UnityEditor;
using UnityEngine;

namespace Common.Ragdoll
{
    [CustomEditor(typeof(Ragdoll))]
    public class RagdollManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var me = target as Ragdoll;

            EU.Label("Ragdoll", Color.white, 'c', true);
            GUILayout.BeginHorizontal();
            if (EU.ButtonMidSize("Get", Color.cyan))
            {
                me.GetAllParts();
                EditorUtility.SetDirty(me);
            }
            if (EU.ButtonMidSize("On", Color.green))
            {
                 me.Activate();   
                 EditorUtility.SetDirty(me);
            }
            if (EU.ButtonMidSize("Off", Color.red))
            {
                me.Deactivate();   
                EditorUtility.SetDirty(me);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space((10));
            
            EU.Label("Interpolate", Color.white, 'c', true);
            GUILayout.BeginHorizontal();
            if (EU.ButtonSmall("I", Color.green))
            {
                me.SetAllInterpolate();   
                EditorUtility.SetDirty(me);
            }
            if (EU.ButtonSmall("E", Color.yellow))
            {
                me.SetAllExtrapolate();   
                EditorUtility.SetDirty(me);
            }
            if (EU.ButtonSmall("N", Color.red))
            {
                me.SetAllNoInterpolate();   
                EditorUtility.SetDirty(me);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.Space((10));

            EU.Label("Projection", Color.white, 'c', true);
            GUILayout.BeginHorizontal();
            if (EU.ButtonSmall("Y", Color.green))
            {
                me.SetProjection();   
                EditorUtility.SetDirty(me);
            }       
            if (EU.ButtonSmall("N", Color.red))
            {
                me.SetNoProjection();   
                EditorUtility.SetDirty(me);
            }
            GUILayout.EndHorizontal();
            
            EU.Label("Preprocessing", Color.white, 'c', true);
            GUILayout.BeginHorizontal();
            if (EU.ButtonSmall("Y", Color.green))
            {
                me.SetAllPreprocess(true);
                EditorUtility.SetDirty(me);
            }       
            if (EU.ButtonSmall("N", Color.red))
            {
                me.SetAllPreprocess(false);
                EditorUtility.SetDirty(me);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space((10));
            base.OnInspectorGUI();
            
       
        }
    }
}
#endif