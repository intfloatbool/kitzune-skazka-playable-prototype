using System;
using UnityEngine;

namespace Prototype.Boss
{
    public class TreeThroat : MonoBehaviour
    {
        [SerializeField] private float _maxScaleY;
        [SerializeField] private float _minScaleY;
        [SerializeField] private float _speed = 6f;
        [SerializeField] private float _speedIncreaseByTentacleEaten = 2f;

        private float _basicScaleY;

        private TentacleThroatCollider _throatCollider;

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
            _speed += _speedIncreaseByTentacleEaten;
        }

        private void Start()
        {
            _basicScaleY = transform.localScale.y;
        }

        private void Update()
        {
            if (_minScaleY > _maxScaleY)
            {
                _minScaleY = _maxScaleY - 0.02f;
            }

            if (_minScaleY < 0)
            {
                _minScaleY = 0;
            }
            
            var currentScale = transform.localScale;
            var pingPong = Mathf.PingPong(Time.time * _speed, _maxScaleY - _minScaleY) + _minScaleY;

            pingPong = Mathf.Clamp(pingPong, _minScaleY, _maxScaleY);

            currentScale.y = pingPong;
            
            transform.localScale = currentScale;
        }
    }
}
