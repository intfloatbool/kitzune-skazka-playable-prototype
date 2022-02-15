using System;
using DG.Tweening;
using UnityEngine;

namespace Prototype.GameUI
{
    public class SceneEndUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _time = 0.5f;
        private Tween _tween;
        
        public void StartHide()
        {
            _canvasGroup.DOFade(1f, _time);
        }

        private void OnDestroy()
        {
            _tween.Kill();
        }
    }
}
