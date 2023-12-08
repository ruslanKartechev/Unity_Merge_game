using Common.Utils;
using UnityEngine;

namespace Creatives.Office
{
    public class ElevatorTrigger : MonoBehaviour, IHumanTrigger
    {
        [SerializeField] private GameObject _elevatorGO;
        private bool _triggerred;
        
        public void OnEntered()
        {
            CLog.LogRed("Entered called");
            if (_triggerred)
                return;
            CLog.LogRed("Triggerred");
            _triggerred = true;
            if (_elevatorGO.TryGetComponent<IElevator>(out var elevator))
            {
                CLog.LogRed("Called to close");
                elevator.Close();
            }
            else
                CLog.LogRed("No elevator");
        }
    }
}