using System.Collections.Generic;

namespace Game.Levels
{
    public class AnalyticsEvents
    {
        public static string boss => "boss";
        public static string normal => "normal";
        
        public void OnStarted(string levelType)
        {
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_start", new Dictionary<string, object>()
            {
                {"level_number", GC.PlayerData.LevelTotal+1},
                {"level_name", "level_"},
                {"level_diff ", "easy"},
                {"level_type", levelType},
            });   
        }
        
        public void OnWin(string levelType)
        {
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_finish", new Dictionary<string, object>()
            {
                {"level_number", GC.PlayerData.LevelTotal+1},
                {"result", "win"},
                {"level_name", "level_"},
                {"level_diff ", "easy"},
                {"level_type", levelType},
                
            });    
        }

        public void OnFailed(string levelType)
        {
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_finish", new Dictionary<string, object>()
            {
                {"level_number", GC.PlayerData.LevelTotal+1},
                {"result", "lose"},
                {"level_name", "level_"},
                {"level_diff ", "easy"},
                {"level_type", levelType},
            });    
        }

        public void OnTutorial(string stepName)
        {
            MadPixelAnalytics.AnalyticsManager.CustomEvent("tutorial", new Dictionary<string, object>()
            {
                {"step_name", stepName},
            });   
        }
        
    }
}