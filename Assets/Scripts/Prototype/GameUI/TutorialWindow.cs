using System;
using DG.Tweening;
using UnityEngine;

namespace Prototype.GameUI
{
    public class TutorialWindow : MonoBehaviour
    {
        [SerializeField] private Transform _tutorialWindow;
        [SerializeField] private Transform _tutorialRoot;

        private bool _isShow = false;

        public Action OnHide { get; set; }
        
        private void Awake()
        {
            Hide();
        }

        [ContextMenu("ShowTutorial")]
        public void ShowTutorial()
        {
            _tutorialRoot.gameObject.SetActive(true);
            _tutorialWindow.localScale = Vector3.zero;
            _tutorialWindow.DOScale(Vector3.one, 0.5f);
            _isShow = true;
        }

        public void Hide()
        {
            _tutorialRoot.gameObject.SetActive(false);
            _tutorialWindow.localScale = Vector3.zero;
            _isShow = false;
            
            OnHide?.Invoke();
        }

        private void Update()
        {
            if(!_isShow)
                return;

            var input = Enum.GetNames(typeof(KeyCode));
            for (int i = 0; i < input.Length; i++)
            {
                if (Input.GetKeyDown((KeyCode) i))
                {
                    Hide();
                }
            }
        }
    }
}
