using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Prototype
{
    [RequireComponent(typeof(Button))]
    public class ReloadCurrentSceneButton : MonoBehaviour
    {
        private bool _isClicked = false;
        private void Awake()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if(_isClicked)
                return;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            _isClicked = true;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) 
                OnClick();
        }
    }
}