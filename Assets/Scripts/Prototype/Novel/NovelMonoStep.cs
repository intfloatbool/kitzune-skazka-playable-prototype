using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Prototype.Novel
{
    public class NovelMonoStep : MonoBehaviour
    {
        public Action<NovelMonoStep> OnDoneCallback { get; set; }

        [SerializeField] private Transform[] _content;

        [SerializeField] private UnityEvent _onHide;

        public virtual void SetActiveContent(bool isActive)
        {
            foreach (var content in _content)
            {
                content.gameObject.SetActive(isActive);
            }
        }

        public virtual void Show()
        {
            SetActiveContent(true);
        }

        public virtual void Hide()
        {
            SetActiveContent(false);
            _onHide?.Invoke();
        }

        public virtual void MakeDone()
        {
            OnDoneCallback?.Invoke(this);
        }
    }
}