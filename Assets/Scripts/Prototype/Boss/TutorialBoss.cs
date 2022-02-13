using System;
using UnityEngine;

namespace Prototype.Boss
{
    public class TutorialBoss : MonoBehaviour
    {
        [SerializeField] private Transform _eye;

        [SerializeField] private float _eyeChangeTime = 2f;

        private float _eyeChangeTimerLoop;

        private void Update()
        {
            _eyeChangeTimerLoop += Time.deltaTime;
            if (_eyeChangeTimerLoop > _eyeChangeTime)
            {
                MoveEye();
                _eyeChangeTimerLoop = 0f;
            }
        }

        private void MoveEye()
        {
            
        }
    }
}
