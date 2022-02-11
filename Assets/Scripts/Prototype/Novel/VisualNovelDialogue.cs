using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Novel
{
    public class VisualNovelDialogue : MonoBehaviour
    {
        [SerializeField] private NovelMonoStep[] _steps;

        private Queue<NovelMonoStep> _stepsQueue;

        public Action<VisualNovelDialogue> OnDialogDoneCallback { get; set; }

        private bool _isDone = false;

        private void OnValidate()
        {
            _steps = GetComponentsInChildren<NovelMonoStep>();
        }

        private void Awake()
        {
            _stepsQueue = new Queue<NovelMonoStep>(_steps);

            foreach (var step in _steps)
            {
                step.gameObject.SetActive(false);
            }
        }

        public void StartDialog()
        {
            MoveNext();
        }

        private void MoveNext()
        {
            if(_isDone)
                return;

            if (_stepsQueue.Count <= 0)
            {
                _isDone = true;
                OnDialogDoneCallback?.Invoke(this);
                return;
            }
            
            var currentStep = _stepsQueue.Dequeue();
            currentStep.gameObject.SetActive(true);
            currentStep.Show();
            currentStep.OnDoneCallback = OnCurrentStepDoneCallback; 
        }

        private void OnCurrentStepDoneCallback(NovelMonoStep currentStep)
        {
            currentStep.OnDoneCallback = null;
            currentStep.Hide();
            
            MoveNext();
        }
    }
}