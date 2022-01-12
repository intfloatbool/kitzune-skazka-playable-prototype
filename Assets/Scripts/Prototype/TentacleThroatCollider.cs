using System;
using System.Collections.Generic;
using Prototype.Boss;
using UnityEngine;

namespace Prototype
{
    public class TentacleThroatCollider : GameWinCollider
    {
        private LinkedList<Tentacle> _currentTentacles;
        public IReadOnlyCollection<Tentacle> CurrentTentacles => _currentTentacles;
        
        public Action OnTentacleEatenCallback { get; set; }

        private void Start()
        {
            _currentTentacles = new LinkedList<Tentacle>(FindObjectsOfType<Tentacle>());
        }

        protected override void OnTriggered(TriggerableObject triggerableObject, Collider2D collider)
        {
            var tentacle = triggerableObject.GetComponentInParent<Tentacle>();
            if (tentacle)
            {
                _currentTentacles.Remove(tentacle);
                tentacle.Kill();
                OnTentacleEatenCallback?.Invoke();
            }
            else
            {
                Debug.LogError("Tentacle is missing!");
            }
        }
    }
}