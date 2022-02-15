using System;
using Prototype.GameUI;
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

        private SceneEndUI _sceneEndUI;

        private void Awake()
        {
            _sceneEndUI = FindObjectOfType<SceneEndUI>();
        }

        public void InvokeSceneLoading()
        {
            if(_isInvoked)
                return;

            if (_sceneEndUI)
            {
                _sceneEndUI.StartHide();
            }
            GameScenesSwitcher.LoadCustomScene(_sceneName);
            _onSceneStartInvoking?.Invoke();
            
            _isInvoked = true;
        }
    }
}