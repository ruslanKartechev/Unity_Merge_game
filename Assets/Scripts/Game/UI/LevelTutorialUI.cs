using System.Collections;
using Common;
using UnityEngine;

namespace Game.UI
{
    public class LevelTutorialUI : MonoBehaviour
    {
        [SerializeField] private Transform _worldFromPoint;
        [SerializeField] private RectTransform _to;
        [SerializeField] private float _moveTime;
        [SerializeField] private TutorText _tutorMessage;
        
        [SerializeField] private string _aimText;
        [SerializeField] private string _jumpText;
        
        private Coroutine _working;
        

        public void BeginAimTutor()
        {
            gameObject.SetActive(true);
            _tutorMessage.Show(_aimText);
            Hand.MoveFromTo(Camera.main.WorldToScreenPoint(_worldFromPoint.position), _to.position, _moveTime);
        }

        public void BeginJumpTutor()
        {
            gameObject.SetActive(true);
            _tutorMessage.Show(_jumpText);
            Hand.ShowRelease();
        }

        public void HideAll()
        {
            Hand.Hide(true);
            _tutorMessage.Hide();
        }

        
        private void StopWorking()
        {
            if(_working != null)
                StopCoroutine(_working);
        }
        
        

        
    }
}