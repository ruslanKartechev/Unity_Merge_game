﻿#if UNITY_EDITOR
using EditorUtils;
using UnityEditor;
using UnityEngine;

namespace Game.Saving
{
    [CustomEditor(typeof(GameDataSaver))]
    public class GameDataSaverEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as GameDataSaver;
            if(EU.ButtonBig("Path", Color.cyan))
                me.LogPath();   
            if(EU.ButtonBig("Clear", Color.red))
                me.Clear();
            if(EU.ButtonBig("Load", Color.green))
                me.Load();
            if(EU.ButtonBig("Save", Color.green))
                me.Save();
        }
    }
}
#endif