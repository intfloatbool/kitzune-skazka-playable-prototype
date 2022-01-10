using UnityEngine;

namespace Prototype
{
    public class ActivateablesController : MonoBehaviour
    {
        [SerializeField] private ActivateableBase[] _activateableCollection;
        [SerializeField] private bool _defaultActivationState = true;

        
        private void Start()
        {
            SetActiveToAll(_defaultActivationState);
        }

        private void OnValidate()
        {
            _activateableCollection = GetComponentsInChildren<ActivateableBase>();
        }

        public void SetActiveToAll(bool isActive)
        {
            foreach (var activateable in _activateableCollection)
            {
                if(activateable)
                    activateable.SetActive(isActive);
            }
        }
    }
}