using System;
using System.Collections.Generic;
using DG.Tweening;
using Prototype.GameUI;
using UnityEngine;

namespace Prototype.Boss
{
    public class TutorialBoss : ActivateableBase
    {
        [System.Serializable]
        private class EyeData
        {
            public Vector3 localPos;
            public Vector3 localScale;
        }
        
        [SerializeField] private Transform _eye;
        [SerializeField] private EyeData[] _eyeDataCollection;
        [SerializeField] private float _eyeChangeTime = 2f;
        [SerializeField] private Transform _soul;
        public Transform Soul => _soul;
        public Animator Animator { get; private set; }

        private float _eyeChangeTimerLoop;

        private Stack<EyeData> _eyeDataStack;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            var shuffled = new List<EyeData>(_eyeDataCollection);
            shuffled.Shuffle();
            _eyeDataStack = new Stack<EyeData>(shuffled);
        }

        private void Update()
        {
            if(!_isActive)
                return;
            
            _eyeChangeTimerLoop += Time.deltaTime;
            if (_eyeChangeTimerLoop > _eyeChangeTime)
            {
                MoveEye();
                _eyeChangeTimerLoop = 0f;
            }
        }

        private void MoveEye()
        {
            if (_eyeDataStack.Count <= 0)
            {
                var shuffled = new List<EyeData>(_eyeDataCollection);
                shuffled.Shuffle();
                _eyeDataStack = new Stack<EyeData>(shuffled);
            }

            var next = _eyeDataStack.Pop();
            _eye.DOLocalMove(next.localPos, _eyeChangeTime);
            _eye.DOScale(next.localScale, _eyeChangeTime);
        }
    }
}
