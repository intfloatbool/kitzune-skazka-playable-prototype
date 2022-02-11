using Prototype.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Prototype.Common
{
    public class MonoSceneLoadingInvoker : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        private bool _isInvoked = false;

        [SerializeField] private UnityEvent _onSceneStartInvoking;

        public void InvokeSceneLoading()
        {
            if(_isInvoked)
                return;

            GameScenesSwitcher.LoadCustomScene(_sceneName);
            _onSceneStartInvoking?.Invoke();
            
            _isInvoked = true;
        }
    }
}