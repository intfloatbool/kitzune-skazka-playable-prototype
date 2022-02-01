using System;
using Prototype.GameScenes;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype.GameUI
{
    [RequireComponent(typeof(Button))]
    public class MainMenuStartButton : MonoBehaviour
    {
        [SerializeField] private string _nextSceneName;

        private bool _isTransitionStarted = false;
        private void Awake()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GoToNextScene();
        }

        private void GoToNextScene()
        {
            if(_isTransitionStarted)
                return;
            GameScenesController.Instance.LoadSceneByName(_nextSceneName);
            _isTransitionStarted = true;
        }

        private void Update()
        {
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
