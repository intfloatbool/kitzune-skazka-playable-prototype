using System;
using UnityEngine;

namespace Prototype.Boss
{
    public class TreeThroat : DynamicGameObject
    {
        
        private TentacleThroatCollider _throatCollider;

        [SerializeField] private float _animationSpeedIncreaseByTentacleEaten = 1f;
        [SerializeField] private Animator _animator;
        private readonly string speedKey = "Speed";
        private float _basicSpeed;
        
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

            _basicSpeed = _animator.GetFloat(speedKey);
        }

        private void OnDestroy()
        {
            if (_throatCollider)
            {
                _throatCollider.OnTentacleEatenEv -= ThroatColliderOnTentacleEaten;
            }
        }

        private void Update()
        {
            if (!_isActive)
            {
                _animator.SetFloat(speedKey, 0);
            }
        }

        private void ThroatColliderOnTentacleEaten()
        {
            var newSpeed = _animator.GetFloat(speedKey) + _animationSpeedIncreaseByTentacleEaten;
            _animator.SetFloat(speedKey, newSpeed);
        }
        
    }
}
