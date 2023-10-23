using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    public class PreyMovePath : MonoBehaviour, IMovePath
    {
        [SerializeField] private List<Transform> _waypoints;
        private int _index;
#if UNITY_EDITOR
        [Header("Debug")] 
        [SerializeField] private float gizmoRadius = 0.25f;
        [SerializeField] private Color gizmoColor;
        [SerializeField] private bool doDraw;
        [SerializeField] private float offsetUp;

        private void OnDrawGizmos()
        {
            if (doDraw == false || _waypoints.Count <= 1)
                return;
            var oldColor = Gizmos.color;
            Gizmos.color = gizmoColor;
            for (var i = 1; i < _waypoints.Count; i++)
            {
                Gizmos.DrawSphere(_waypoints[i-1].position + Vector3.up * offsetUp, gizmoRadius);
                Gizmos.DrawLine(_waypoints[i].position + Vector3.up * offsetUp, _waypoints[i-1].position + Vector3.up * offsetUp);
            }
            Gizmos.DrawSphere(_waypoints[^1].position + Vector3.up * offsetUp, gizmoRadius);
            Gizmos.DrawLine(_waypoints[^1].position + Vector3.up * offsetUp, _waypoints[0].position + Vector3.up * offsetUp);
            Gizmos.color = oldColor;
        }
#endif


        public Vector3 GetPosition(int i)
        {
            return _waypoints[i].position;
        }

        public void NextPosition()
        {
            _index++;
            if (_index >= _waypoints.Count)
                _index = 0;
        }

        public Vector3 GetCurrentPosition()
        {
            return _waypoints[_index].position;
        }
    }

    public interface IMovePath
    {
        Vector3 GetPosition(int i);
        void NextPosition();
        Vector3 GetCurrentPosition();
    }
}