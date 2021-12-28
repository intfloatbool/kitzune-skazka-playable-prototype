using UnityEngine;

namespace Prototype.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputController _inputController;
        private float _moveSpeed;

        private void Start()
        {
            _moveSpeed = GameManager.ValuesProvider.GetFloat("PLAYER_MOVE_SPEED");
        }

        private void Update()
        {
            if (_inputController)
            {
                var moveDirection = new Vector3(
                    _inputController.Horizontal,
                    _inputController.Vertical,
                    0
                );
                transform.Translate(moveDirection * _moveSpeed * Time.deltaTime);
            }
            else
            {
                Debug.LogError("InputController is missing!");
            }
        }
    }
}
