using System;
using System.Collections;
using Prototype.GameScenes;
using Prototype.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype.GameUI
{
    [RequireComponent(typeof(Button))]
    public class MainMenuStartButton : MonoBehaviour
    {
        [SerializeField] private string _nextSceneName;
        [SerializeField] private float _delayToActive = 1.5f;

        private bool _isActive = false;
        
        private bool _isTransitionStarted = false;
        private void Awake()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_delayToActive);
            _isActive = true;
        }
        

        private void OnClick()
        {
            GoToNextScene();
        }

        private void GoToNextScene()
        {
            if(!_isActive)
                return;
            
            if(_isTransitionStarted)
                return;
            GameScenesSwitcher.LoadCustomScene(_nextSceneName);
            _isTransitionStarted = true;
        }

        private void Update()
        {
            if(!_isActive)
                return;
            if(_isTransitionStarted)
                return;
            var keyNames = Enum.GetNames(typeof(KeyCode));
            for (int i = 0; i < keyNames.Length; i++)
            {
                if (Input.GetKeyDown((KeyCode) i))
                {
                    GoToNextScene();
                }
            }
        }
    }
}
