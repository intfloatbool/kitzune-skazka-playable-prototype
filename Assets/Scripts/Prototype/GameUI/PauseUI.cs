using System;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype.GameUI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _exitButton;

        private void Awake()
        {
            _continueButton.onClick.AddListener(OnContinueClick);
            _exitButton.onClick.AddListener(OnExitClick);
        }

        private void OnEnable()
        {
            GameHelper.SetTimeScale(0f);
        }

        private void OnContinueClick()
        {
              GameHelper.SetTimeScale(1f);
              gameObject.SetActive(false);
        }

        private void OnExitClick()
        {
            Application.Quit();
        }
    }
}
