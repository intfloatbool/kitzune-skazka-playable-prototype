using System;
using Prototype.Managers;
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

        [SerializeField] private GameObject _normalBody;
        [SerializeField] private GameObject _deadBody;

        private void Start()
        {
            _normalBody.SetActive(true);
            _deadBody.SetActive(false);
            
            OnPlayerCreated?.Invoke(this);
        }

        public void MakeImmortal()
        {
            _isPlayerImmortal = true;
        }

        public void MakeMortal()
        {
            _isPlayerImmortal = false;
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
            
            _normalBody.SetActive(false);
            _deadBody.SetActive(true);
            _isDead = true;
            
            SoundManager.PlaySound("kitsune_death");
        }
    }
}
