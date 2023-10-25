using UnityEngine;

namespace Game.Hunting
{
    public class FishTrackModeSetter : MonoBehaviour
    {
        [SerializeField] private Transform _fishTank;
        [Space(10)] 
        [SerializeField] private Vector3 _posOnLand;
        [SerializeField] private Vector3 _posOnWater;

        public void SetLand()
        {
            _fishTank.localPosition = _posOnLand;
        }

        public void SetWater()
        {
            _fishTank.localPosition = _posOnWater;
        }
        
        
        
    }
}