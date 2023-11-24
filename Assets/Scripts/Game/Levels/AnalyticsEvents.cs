#define SEND_EVENTS__
using System.Collections.Generic;
using UnityEngine;

namespace Game.Levels
{
    public class AnalyticsEvents
    {
        public static string boss => "boss";
        public static string normal => "normal";

        public static void OnStarted(string levelType)
        {
            Debug.Log($"[Analytics] Start: {GC.PlayerData.LevelTotal + 1}");
            #if SEND_EVENTS
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_start", new Dictionary<string, object>()
            {
                { "level_number", GC.PlayerData.LevelTotal + 1 },
                { "level_name", "level_" },
                { "level_diff ", "easy" },
                { "level_type", levelType },
            });
            #endif
        }
        
        public static void OnFinished(string levelType)
        {
            Debug.Log($"[Analytics] Finish: {GC.PlayerData.LevelTotal + 1}");
#if SEND_EVENTS
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_finish", new Dictionary<string, object>()
            {
                {"level_number", GC.PlayerData.LevelTotal+1},
                {"result", "win"},
                {"level_name", "level_"},
                {"level_diff ", "easy"},
                {"level_type", levelType},
            });    
            #endif
        }

        public static void OnFailed(string levelType)
        {
            Debug.Log($"[Analytics] Fail: {GC.PlayerData.LevelTotal + 1}");
#if SEND_EVENTS
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_finish", new Dictionary<string, object>()
            {
                {"level_number", GC.PlayerData.LevelTotal+1},
                {"result", "lose"},
                {"level_name", "level_"},
                {"level_diff ", "easy"},
                {"level_type", levelType},
            });      
            #endif
        }

        public static void OnTutorial(string stepName)
        {
#if SEND_EVENTS
            MadPixelAnalytics.AnalyticsManager.CustomEvent("tutorial", new Dictionary<string, object>()
            {
                {"step_name", stepName},
            });
            #endif
        }
        
        public static void OnRestart(string levelType)
        {
            Debug.Log($"[Analytics] Restart: {GC.PlayerData.LevelTotal + 1}");
#if SEND_EVENTS
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_finish", new Dictionary<string, object>()
            {
                {"level_number", GC.PlayerData.LevelTotal+1},
                {"result", "restart"},
                {"level_name", "level_"},
                {"level_diff ", "easy"},
                {"level_type", levelType},
            });    
            #endif
        }
        
        public static void OnExited(string levelType)
        {
            Debug.Log($"[Analytics] Exit: {GC.PlayerData.LevelTotal + 1}");
#if SEND_EVENTS
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_finish", new Dictionary<string, object>()
            {
                {"level_number", GC.PlayerData.LevelTotal+1},
                {"result", "exit"},
                {"level_name", "level_"},
                {"level_diff ", "easy"},
                {"level_type", levelType},
            });    
            #endif
        }

        public static void OnEggPurchase(int level, float cost, string item)
        {
#if SEND_EVENTS
            MadPixelAnalytics.AnalyticsManager.CustomEvent("egg_purchase", new Dictionary<string, object>()
            {
                { "egg_level", level},
                { "egg_cost", cost },
                { "item ", item }
            });
            #endif
        }

        public static void OnGridCellPurchase(int x, int y, float cost)
        {
#if SEND_EVENTS
            MadPixelAnalytics.AnalyticsManager.CustomEvent("grid_cell_purchase", new Dictionary<string, object>()
            {
                { "cost", cost},
                { "x", x },
                { "y ", y }
            });
            #endif
        }
    }
}