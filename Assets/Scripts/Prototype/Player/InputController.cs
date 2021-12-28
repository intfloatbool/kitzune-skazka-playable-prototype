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

            Horizontal = x;
            Vertical = y;
        }
    }
}
