﻿using System;
using System.Collections.Generic;
using Prototype.Boss;
using Prototype.Managers;
using UnityEngine;

namespace Prototype
{
    public class TentacleThroatCollider : GameWinCollider
    {
        private LinkedList<Tentacle> _currentTentacles;
        public IReadOnlyCollection<Tentacle> CurrentTentacles => _currentTentacles;
        
        public Action OnTentacleEatenCallback { get; set; }

        private bool _isActive = true;
        
        public event Action OnTentacleEatenEv;

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }
        
        private void Start()
        {
            _currentTentacles = new LinkedList<Tentacle>(FindObjectsOfType<Tentacle>());
        }

        protected override void OnTriggered(TriggerableObject triggerableObject, Collider2D collider)
        {
            if(!_isActive)
                return;
            
            var tentacle = triggerableObject.GetComponentInParent<Tentacle>();
            if (tentacle)
            {
                EatTentacle(tentacle);
            }
            else
            {
                Debug.LogError("Tentacle is missing!");
            }
        }

        public void EatTentacle(Tentacle tentacle)
        {
            _currentTentacles.Remove(tentacle);
            tentacle.Kill();
            OnTentacleEatenCallback?.Invoke();
            OnTentacleEatenEv?.Invoke();
            
            SoundManager.PlaySound("tentacle_eaten");
        }
    }
}