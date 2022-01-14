using UnityEngine;

namespace Prototype.Player
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;

        public float Horizontal { get; private set; }
        
        public float Vertical { get; private set; }

        private readonly string horizontalAxisKey = "Horizontal";
        private readonly string verticalAxisKey = "Vertical";

        private void Update()
        {
            float x = Input.GetAxis(horizontalAxisKey);
            float y = Input.GetAxis(verticalAxisKey);

            x += _joystick.Horizontal;
            y += _joystick.Vertical;
            
            
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                x = 0f;
                Debug.Log("LR arrow up");
            }
                
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                x = 0f;
                Debug.Log("AD up");
            }
                
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                y = 0f;
                Debug.Log("UD up");
            }
                
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                y = 0f;
                Debug.Log("WS up");
            }
            
            Horizontal = x;
            Vertical = y;
        }
    }
}
