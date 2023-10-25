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
        private bool _logAllLines = false;
        
        private string DataPath
        {
            get
            {
                return Application.dataPath.TrimEnd(new[]{'A', 's', 's','e', 't', 's'});
            }
        }
        
        
        [ContextMenu("Fix Animation")]
        public void Fix()
        {
            var filePath = GetPathToAsset(_clip);
            CLog.LogWHeader($"AnimationFixer", $"Path: {filePath}", "w");
            if (File.Exists(filePath) == false)
            {
                CLog.LogWHeader($"AnimationFixer", $"Does not exist", "w");
                return;
            }

            // var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
            var lines = File.ReadAllLines(filePath);
            var correctedLines = new List<string>();
            var correctedLinesCount = 0;
            foreach (var line in lines)
            {
                if (line.Contains(_wordToRemove))
                {
                    CLog.LogWHeader("Corrected", $"{line}", "g", "w");
                    var correctedLine = line.Replace(_wordToRemove, "");
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
            // WriteToFile(correctedLines, fileStream);
            File.WriteAllLines(filePath, correctedLines);
            // stream.Close();
        }
        
        private string GetPathToAsset(Object obj)
        {
            return DataPath + AssetDatabase.GetAssetPath(obj);
        }
    }
}