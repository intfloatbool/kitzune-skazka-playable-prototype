using System;
using UnityEngine;
using UnityEngine.Events;

namespace Prototype.Player
{
    public class FoxPlayer : MonoBehaviour
    {
        
        public static event Action<FoxPlayer> OnPlayerCreated;
        
        public event Action OnDead;
        [SerializeField] private UnityEvent _onDead;
        private bool _isDead = false;
        public bool IsDead => _isDead;
        
        [Space(10f)]
        [SerializeField] private bool _isPlayerImmortal;

        private void Start()
        {
            OnPlayerCreated?.Invoke(this);
        }

        public void MakeImmortal()
        {
            _isPlayerImmortal = true;
        }

        public void Kill()
        {
            if(_isPlayerImmortal)
                return;
            
            if(_isDead)
                return;
            
            Debug.Log("<color=red>PLAYER DEAD.</color>");
            _onDead?.Invoke();
            OnDead?.Invoke();
            _isDead = true;
        }
    }
}
