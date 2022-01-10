using UnityEngine;
using UnityEngine.Events;

namespace Prototype
{
    public class ActivateableBase : MonoBehaviour
    {
        [SerializeField] protected bool _isActive;

        [Space] 
        [SerializeField] private UnityAction _onActivate;
        [SerializeField] private UnityAction _onDeactivate;
        
        public virtual void SetActive(bool isActive)
        {
            _isActive = isActive;

            UnityAction actionToCall = _isActive ? _onActivate : _onDeactivate;
            
            actionToCall?.Invoke();
        }
    }
}