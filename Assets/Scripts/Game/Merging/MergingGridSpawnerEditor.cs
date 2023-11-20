﻿#if UNITY_EDITOR
using Common.Utils.EditorUtils;
using UnityEditor;
using UnityEngine;

namespace Game.Merging
{
    [CustomEditor(typeof(GroupGridBuilder))]
    public class MergingGridSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as GroupGridBuilder;
            if(EU.ButtonBig("Spawn", Color.green))
                 me.Spawn(me.DebugActiveGroup.Group());
            if(EU.ButtonBig("Clear", Color.red))
                me.Clear();

        }
    }
}
#endif