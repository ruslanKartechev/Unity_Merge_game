using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;


namespace Common
{
    public class AnimationFixer : MonoBehaviour
    {
        [SerializeField] private string _wordToRemove = "Armature/";
        [SerializeField] private AnimationClip _clip;
        [SerializeField] private List<AnimationClip> _clips;
        private bool _logAllLines = false;
        
        private string DataPath
        {
            get
            {
                return Application.dataPath.TrimEnd(new[]{'A', 's', 's','e', 't', 's'});
            }
        }

        [ContextMenu("Fix List of Animations")]
        public void FixList()
        {
            foreach (var clip in _clips)
            {
                FixAnimation(clip, _wordToRemove);
            }
        }

        [ContextMenu("Fix Animation")]
        public void Fix()
        {
            FixAnimation(_clip, _wordToRemove);
        }
        
        public void FixAnimation(AnimationClip clip, string token)
        {
            var filePath = GetPathToAsset(clip);
            CLog.LogWHeader($"AnimationFixer", $"Path: {filePath}", "w");
            if (File.Exists(filePath) == false)
            {
                CLog.LogWHeader($"AnimationFixer", $"Does not exist", "w");
                return;
            }
            var lines = File.ReadAllLines(filePath);
            var correctedLines = new List<string>();
            var correctedLinesCount = 0;
            foreach (var line in lines)
            {
                if (line.Contains(token))
                {
                    CLog.LogWHeader("Corrected", $"{line}", "g", "w");
                    var correctedLine = line.Replace(token, "");
                    correctedLines.Add(correctedLine);
                    correctedLinesCount++;
                }
                else
                {
                    correctedLines.Add(line);
                    if(_logAllLines)
                        CLog.Log($"{line}");
                }
            }
            CLog.LogWHeader("AnimationFixer", $"Fixed lines count: {correctedLinesCount}", "g");
            File.WriteAllLines(filePath, correctedLines);
        }
        
        private string GetPathToAsset(Object obj)
        {
            return DataPath + AssetDatabase.GetAssetPath(obj);
        }
    }
}