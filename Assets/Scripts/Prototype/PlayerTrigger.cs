using Prototype.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Prototype
{
    [RequireComponent(typeof(TriggerCollider))]
    public class PlayerTrigger : MonoBehaviour
    {
        [SerializeField] private bool _isOnce;
        private TriggerCollider _triggerCollider;

        [SerializeField] private UnityEvent _onPlayerTirgger;

        private bool _isActive = true;
        
        private void Awake()
        {
            _triggerCollider = GetComponent<TriggerCollider>();
            _triggerCollider.OnTriggerCallback = OnTriggerCallback;

            _isActive = true;
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
                _onPlayerTirgger?.Invoke();

                if (_isOnce)
                {
                    _isActive = false;
                }
            }
        }
    }
}