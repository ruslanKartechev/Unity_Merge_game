#if UNITY_EDITOR
using Common.Utils.EditorUtils;
using UnityEditor;
using UnityEngine;

namespace Creatives.Firemen
{
    [CustomEditor(typeof(KongEnemiesPushBack))]
    public class KongEnemiesPushBackEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as KongEnemiesPushBack;
            if (EU.ButtonLarge("Get Refs", Color.blue))
            {
                me.GetRefs();
            }
        }
    }
}
#endif