using Prototype.Player;
using UnityEngine;

namespace Prototype
{
    [RequireComponent(typeof(TriggerCollider))]
    public class PlayerKillTrigger : ActivateableBase
    {
        private TriggerCollider _triggerCollider;

        private void Awake()
        {
            _triggerCollider = GetComponent<TriggerCollider>();
            _triggerCollider.OnTriggerCallback = OnTriggerCallback;
        }

        private void OnTriggerCallback(TriggerableObject triggerable, Collider2D col2d)
        {
            if(!_isActive)
                return;
            
            if (triggerable.UnitType != UnitType.FOX_PLAYER)
                return;
            var player = triggerable.GetComponent<FoxPlayer>();
            if (player)
            {
                player.Kill();
            }
        }
    }
}
