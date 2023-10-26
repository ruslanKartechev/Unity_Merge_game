using UnityEngine;

namespace Game.WorldMap
{
    [System.Serializable]
    public class WorldMapCameraPoint
    {
        [SerializeField] private Transform _point;
        [SerializeField] private Vector3 _offset;

        public Vector3 Offset
        {
            get => _offset;
            set => _offset = value;
        }

        public Transform Point
        {
            get => _point;
            set => _point = value;
        }
    }
}