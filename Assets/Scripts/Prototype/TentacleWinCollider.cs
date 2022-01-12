using System;
using System.Collections.Generic;
using Prototype.Boss;
using UnityEngine;

namespace Prototype
{
    public class TentacleWinCollider : GameWinCollider
    {
        private LinkedList<Tentacle> _currentTentacles;

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
                Destroy(tentacle.gameObject);

                foreach (var existedTentacle in _currentTentacles)
                {
                    if (existedTentacle.CurrentState == Tentacle.TentacleState.ATTACK)
                    {
                        existedTentacle.RestMove();
                    }
                }
                
                if (_currentTentacles.Count <= 0)
                {
                    GameManager.Instance.GameWin();
                }
            }
            else
            {
                Debug.LogError("Tentacle is missing!");
            }
        }
    }
}