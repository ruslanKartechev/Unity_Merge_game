#if UNITY_EDITOR
using EditorUtils;
using UnityEditor;
using UnityEngine;

namespace Game.Hunting
{
    [CustomEditor(typeof(PreyBarbarian))]
    public class PreyBarbarianEditor : Editor
    {
        private PreyBarbarian _me;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _me = target as PreyBarbarian;
            GUILayout.Space(10);
            EU.Label("Editor only behaviour switcher", 'c', true);
            EU.TwoButtonAndLabel("<<", ">>", "Idle Behaviour", 
                Color.cyan, Color.cyan, Color.green,
                () => { _me.SwitchIdle(false);}, () => { _me.SwitchIdle(true);});

            EU.TwoButtonAndLabel("<<", ">>", "Surprised Behaviour", 
                Color.cyan, Color.cyan, Color.green,
                () => { _me.SwitchSurprised(false);}, () => { _me.SwitchSurprised(true);});
            
            EU.TwoButtonAndLabel("<<", ">>", "Run Behaviour", 
                Color.cyan, Color.cyan, Color.green,
                () => { _me.SwitchRun(false);}, () => { _me.SwitchRun(true);});
            
            EU.TwoButtonAndLabel("<<", ">>", "Dead Behaviour", 
                Color.cyan, Color.cyan, Color.green,
                () => { _me.SwitchDead(false);}, () => { _me.SwitchDead(true);});
        }
    }
}
#endif