using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Game.Hunting
{
    public class RagdollPositioner : MonoBehaviour
    {
        [SerializeField] private List<Transform> _bones;
        [SerializeField] private List<Data> _dataPoints;

        
        [ContextMenu("SetPosition")]
        public void SetPosition()
        {
            foreach (var point in _dataPoints)
                point.Set();
        }
        
        [ContextMenu("SavePositions")]
        public void SavePositions()
        {
            _dataPoints = new List<Data>(_bones.Count);
            foreach (var pp in _bones)
            {
                var data = new Data()
                {
                    bone = pp,
                    localPos = pp.localPosition,
                    localEulers = pp.localEulerAngles,
                };
                _dataPoints.Add(data);
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        #if UNITY_EDITOR
        [ContextMenu("DebugSetPosition")]
        public void DebugSetBones()
        {
            EditorUtility.SetDirty(this);
            for (var i = 0; i < _dataPoints.Count; i++)
                _dataPoints[i].bone = _bones[i];
        }
        #endif
        

        [System.Serializable]
        public class Data
        {
            public Transform bone;
            public Vector3 localPos;
            public Vector3 localEulers;

            public void Set()
            {
                bone.localPosition = localPos;
                bone.localEulerAngles = localEulers;
            }
        }
    }
}