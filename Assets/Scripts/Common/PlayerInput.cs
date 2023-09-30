using System.Collections;
using UnityEngine;

namespace Common
{
    [DefaultExecutionOrder(-1000)]
    public class PlayerInput : MonoBehaviour, IPlayerInput
    {
        private bool _active;
        private bool _click;
        private bool _released;
        private bool _pressed;
        
        private Coroutine _working;
        
        public bool IsDown() => _active && _click;

        public bool IsUp() => _active && _released;

        public bool IsPressed() => _active && _pressed;

        public Vector3 MousePosition()
        {
            return Input.mousePosition;
        }

        public void Enable()
        {
            Debug.Log("Input Enabled");
            _active = true;
            StopCor();
            _working = StartCoroutine(Working());
        }

        public void Disable()
        {
            Debug.Log("Input Disabled");
            _active = false;
            StopCor();
        }

        private void StopCor()
        {
            if(_working != null)
                StopCoroutine(_working);
        }

        private IEnumerator Working()
        {
            while (true)
            {
                _click =_pressed = _released = false;
                if (Input.GetMouseButtonDown(0))
                    _click = true;
                if (Input.GetMouseButton(0))
                    _pressed = true;
                if (Input.GetMouseButtonUp(0))
                    _released = true;

                yield return null;
            }
        }
    }
}