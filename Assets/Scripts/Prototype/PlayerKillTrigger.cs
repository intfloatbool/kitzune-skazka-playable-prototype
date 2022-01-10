using Prototype.Player;
using UnityEngine;

namespace Prototype
{
    [RequireComponent(typeof(TriggerCollider))]
    public class PlayerKillTrigger : MonoBehaviour
    {
        private TriggerCollider _triggerCollider;

        private void Awake()
        {
            _triggerCollider = GetComponent<TriggerCollider>();
            _triggerCollider.OnTriggerCallback = OnTriggerCallback;
        }

        private void OnTriggerCallback(TriggerableObject triggerable, Collider2D col2d)
        {
            var player = triggerable.GetComponent<FoxPlayer>();
            if (player)
            {
                player.Kill();
            }
        }
    }
}
