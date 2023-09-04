using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Utils
{
   
    public static class HierarchyUtils
    {

        public static AT GetOrAdd<AT, AB>(this GameObject root) where AT : Component where AB : AT
        {
            var t = root.GetComponent<AT>();
            if (t == null)
                t = root.AddComponent<AB>();
            return t;
        }
        
        public static T GetOrAdd<T>(this GameObject root) where T: Component
        {
            var t = root.GetComponent<T>();
            if (t == null)
                t = root.AddComponent<T>();
            return t;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static List<T> GetFromAllChildren<T>(Transform parent, Condition<T> condition = null)
        {
            var list = new List<T>();
            var t = parent.GetComponent<T>();
            if (t != null)
            {
                if((condition != null && condition(t)) 
                   || condition == null)
                    list.Add(t);
            }
            GetFromChildrenAndAdd(list, parent.transform, condition);
            
            return list;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void GetFromChildrenAndAdd<T>(ICollection<T> list, Transform parent, Condition<T> condition = null)
        {
            if (parent.childCount == 0)
                return;
            for (var i = 0; i < parent.childCount; i++)
            {
                var target = parent.GetChild(i).GetComponent<T>();
                if (target != null )
                {
                    if((condition != null && condition(target)) 
                       || condition == null)
                        list.Add(target);
       
                }
                GetFromChildrenAndAdd<T>(list, parent.GetChild(i), condition);
            }
        }
    }
}