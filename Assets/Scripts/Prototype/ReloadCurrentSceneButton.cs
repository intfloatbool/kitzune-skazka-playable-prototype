using System;
using Prototype.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Prototype
{
    [RequireComponent(typeof(Button))]
    public class ReloadCurrentSceneButton : MonoBehaviour
    {
        private bool _isClicked = false;
        private Button _btn;

        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _disactiveColor;
        
        private void Awake()
        { 
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if(_isClicked)
                return;
            
            GameScenesSwitcher.LoadCustomScene(SceneManager.GetActiveScene().name);
            _isClicked = true;
        }

        public void SetColorToActive()
        {
            _btn.image.color = _activeColor;
        }
        
        public void SetColorToDisactive()
        {
            _btn.image.color = _disactiveColor;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) 
                OnClick();
        }
    }
}