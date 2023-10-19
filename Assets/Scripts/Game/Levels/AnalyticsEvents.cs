﻿using System.Collections.Generic;
using UnityEngine;

namespace Game.Levels
{
    public class AnalyticsEvents
    {
        public static string boss => "boss";
        public static string normal => "normal";
        
        
        public static void OnStarted(string levelType)
        {
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_start", new Dictionary<string, object>()
            {
                { "level_number", GC.PlayerData.LevelTotal + 1 },
                { "level_name", "level_" },
                { "level_diff ", "easy" },
                { "level_type", levelType },
            });
        }
        
        public static void OnFinished(string levelType)
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

        public static void OnFailed(string levelType)
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

        public static void OnTutorial(string stepName)
        {
            MadPixelAnalytics.AnalyticsManager.CustomEvent("tutorial", new Dictionary<string, object>()
            {
                {"step_name", stepName},
            });   
        }
        
        public static void OnRestart(string levelType)
        {
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_finish", new Dictionary<string, object>()
            {
                {"level_number", GC.PlayerData.LevelTotal+1},
                {"result", "restart"},
                {"level_name", "level_"},
                {"level_diff ", "easy"},
                {"level_type", levelType},
            });    
        }
        
        public static void OnExited(string levelType)
        {
            MadPixelAnalytics.AnalyticsManager.CustomEvent("level_finish", new Dictionary<string, object>()
            {
                {"level_number", GC.PlayerData.LevelTotal+1},
                {"result", "exit"},
                {"level_name", "level_"},
                {"level_diff ", "easy"},
                {"level_type", levelType},
            });    
        }

        public static void OnEggPurchase(int level, float cost, string item)
        {
            MadPixelAnalytics.AnalyticsManager.CustomEvent("egg_purchase", new Dictionary<string, object>()
            {
                { "egg_level", level},
                { "egg_cost", cost },
                { "item ", item }
            });
        }

        public static void OnGridCellPurchase(int x, int y, float cost)
        {
            MadPixelAnalytics.AnalyticsManager.CustomEvent("grid_cell_purchase", new Dictionary<string, object>()
            {
                { "cost", cost},
                { "x", x },
                { "y ", y }
            });
        }


        
    }
}