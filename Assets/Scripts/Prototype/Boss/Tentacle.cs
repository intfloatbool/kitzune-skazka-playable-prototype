using System;
using Prototype.Player;
using UnityEngine;

namespace Prototype.Boss
{
    public class Tentacle : MonoBehaviour
    {
        [SerializeField] private TriggerCollider _activateTrigger;
        [SerializeField] private TriggerCollider _killTrigger;

        private void Awake()
        {
            _activateTrigger.OnTriggerCallback = OnActivateTriggerEnter;
            _killTrigger.OnTriggerCallback = OnKillTriggerEnter;
        }

        private void OnActivateTriggerEnter(TriggerableObject triggerable, Collider2D col)
        {
            var player = triggerable.GetComponent<FoxPlayer>();
            if (player)
            {
                   
            }
        }
        
        private void OnKillTriggerEnter(TriggerableObject triggerable, Collider2D col)
        {
            var player = triggerable.GetComponent<FoxPlayer>();
            if (player)
            {
                
            }
        }
        
    }
}
