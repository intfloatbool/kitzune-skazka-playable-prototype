using UnityEngine;

namespace Prototype.Boss
{
    public class TreeThroat : MonoBehaviour
    {
        
        private TentacleThroatCollider _throatCollider;

        [SerializeField] private float _animationSpeedIncreaseByTentacleEaten = 1f;
        [SerializeField] private Animator _animator;
        private readonly string speedKey = "Speed";

        private void Awake()
        {
            _throatCollider = FindObjectOfType<TentacleThroatCollider>();
            if (_throatCollider)
            {
                _throatCollider.OnTentacleEatenEv += ThroatColliderOnTentacleEaten;
            }
            else
            {
                Debug.LogError("ThroatController is missing!");
            }
        }

        private void OnDestroy()
        {
            if (_throatCollider)
            {
                _throatCollider.OnTentacleEatenEv -= ThroatColliderOnTentacleEaten;
            }
        }

        private void ThroatColliderOnTentacleEaten()
        {
            var newSpeed = _animator.GetFloat(speedKey) + _animationSpeedIncreaseByTentacleEaten;
            _animator.SetFloat(speedKey, newSpeed);
        }
        
    }
}
