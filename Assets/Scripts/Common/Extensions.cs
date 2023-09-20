using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class Extensions
    {
        public static T Random<T>(this IList<T> list)
        {
            if (list.Count == 0)
                return default;
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static float Random(this Vector2 vec)
        {
            return UnityEngine.Random.Range(vec.x, vec.y);
        }
    }
}