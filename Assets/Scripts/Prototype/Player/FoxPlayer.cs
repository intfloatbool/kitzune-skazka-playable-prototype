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

        private void Start()
        {
            OnPlayerCreated?.Invoke(this);
        }

        public void Kill()
        {
            if(_isDead)
                return;
            
            Debug.Log("<color=red>PLAYER DEAD.</color>");
            _onDead?.Invoke();
            OnDead?.Invoke();
            _isDead = true;
        }
    }
}
