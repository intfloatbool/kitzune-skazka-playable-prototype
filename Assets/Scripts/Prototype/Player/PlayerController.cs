using UnityEngine;

namespace Prototype.Player
{
    public class PlayerController : ActivateableBase
    {
        [SerializeField] private InputController _inputController;

        [Range(0, 1f)]
        [SerializeField] private float _moveThreshold = 0.5f;
        
        private float _moveSpeed;

        private Vector3 _maxMoveBorder;
        private Vector3 _minMoveBorder;

        private Vector3 _lastPosition;

        private void Start()
        {
            _moveSpeed = GameManager.ValuesProvider.GetFloat("PLAYER_MOVE_SPEED");

            _maxMoveBorder = new Vector3(
                GameManager.ValuesProvider.GetFloat("PLAYER_BORDER_MAX_X"),
                GameManager.ValuesProvider.GetFloat("PLAYER_BORDER_MAX_Y"),
                0
            );
            
            _minMoveBorder = new Vector3(
                GameManager.ValuesProvider.GetFloat("PLAYER_BORDER_MIN_X"),
                GameManager.ValuesProvider.GetFloat("PLAYER_BORDER_MIN_Y"),
                0
            );

            _lastPosition = transform.position;


        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }
            
            if (_inputController)
            {

                var moveDirection = new Vector3(
                    _inputController.Horizontal,
                    _inputController.Vertical,
                    0
                );
                

                if (Mathf.Abs(moveDirection.x) > Mathf.Epsilon || Mathf.Abs(moveDirection.y) > Mathf.Epsilon)
                {
                    var nextPosition = transform.position + moveDirection *  _moveSpeed * Time.deltaTime;
                
                    if (nextPosition.x > _maxMoveBorder.x)
                    {
                        nextPosition.x = _maxMoveBorder.x;
                    }

                    if (nextPosition.x < _minMoveBorder.x)
                    {
                        nextPosition.x = _minMoveBorder.x;
                    } 
                
                    if (nextPosition.y > _maxMoveBorder.y)
                    {
                        nextPosition.y = _maxMoveBorder.y;
                    }

                    if (nextPosition.y < _minMoveBorder.y)
                    {
                        nextPosition.y = _minMoveBorder.y;
                    }


                    transform.position = nextPosition;
                    _lastPosition = nextPosition;
                }
            }
            else
            {
                Debug.LogError("InputController is missing!");
            }
        }
    }
}
