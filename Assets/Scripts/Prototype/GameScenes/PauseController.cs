using System;
using UnityEngine;

namespace Prototype.GameScenes
{
    public class PauseController : MonoBehaviour
    {
        public static PauseController Instance { get; private set; }
        
        [SerializeField] private GameObject _pauseUi;
        [SerializeField] private KeyCode _keyToPause = KeyCode.Escape;
        
        public bool IsActive { get; set; } = true;
        
        private void Awake()
        {
            _pauseUi.gameObject.SetActive(false);
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("Some instance of PauseController is ready!");
            }
        }

        private void Update()
        {
            if(!IsActive)
                return;

            if (Input.GetKeyDown(_keyToPause))
            {
                _pauseUi.gameObject.SetActive(true);
            }
        }
    }
}
