#if UNITY_EDITOR
using UnityEditor;

namespace Game.WorldMap
{
    [CustomEditor(typeof(CamPointController))]
    public class CamPointControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var me = target as CamPointController;
            me.SetCamToThisPoint();
        }
    }
}
#endif