using UnityEngine;

namespace Prototype
{
    public abstract class DynamicGameObject : MonoBehaviour
    {
        protected bool _isActive = true;
        
        public virtual void SetActive(bool isActive)
        {
            _isActive = isActive;
        }
    }
}