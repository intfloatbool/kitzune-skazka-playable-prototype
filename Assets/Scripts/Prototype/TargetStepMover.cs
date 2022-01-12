using System;
using System.Linq;
using UnityEngine;

namespace Prototype
{
    public class TargetStepMover : MonoBehaviour
    {
        [System.Serializable]
        public struct MoveData
        {
            public Vector3 Position;
            public float Speed;
            public float Delay;
        }
        [SerializeField] private int _startMoveIndex;
        [SerializeField] private MoveData[] _moveDataCollection;

        private MoveData _currentMoveData;
        public MoveData CurrentMoveData => _currentMoveData;
        
        [Space]
        [Header("Runtime")]
        [SerializeField] private int _currentMoveDataIndex;
        public int CurrentMoveDataIndex => _currentMoveDataIndex;

        private float _currentMoveDataTimer;

        private bool _isActive;
        
        public Action OnLoopDoneCallback { get; set; }
        
        public void AddMoveData(MoveData moveData)
        {
            var currentCollection = _moveDataCollection.ToList();
            currentCollection.Add(moveData);
            _moveDataCollection = currentCollection.ToArray();
        }
        
        public void SetActiveMove(bool isActive)
        {
            _isActive = isActive;
        }

        public void ResetMover()
        {
            _currentMoveDataIndex = _startMoveIndex;
            SelectMoveData(); 
        }

        private void PickNextMoveData()
        {
            IncreaseNextDataIndex();
            SelectMoveData();
        }

        public void SetCurrentMoveDataIndex(int index)
        {
            _currentMoveDataIndex = index;
            SelectMoveData();
        }

        private void IncreaseNextDataIndex()
        {
            _currentMoveDataIndex++;
            if (_currentMoveDataIndex > _moveDataCollection.Length - 1)
            {
                _currentMoveDataIndex = 0;
                OnLoopDoneCallback?.Invoke();
            }
        }
        
        private void SelectMoveData()
        {
            _currentMoveData = _moveDataCollection[_currentMoveDataIndex];
            _currentMoveDataTimer = 0f;
        }

        private void Update()
        {
            if (!_isActive)
                return;
            
            MoveLoop();
        }

        private void MoveLoop()
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentMoveData.Position, _currentMoveData.Speed * Time.deltaTime);

            float minDistance = 0.01f;

            if (transform.position.Equals(_currentMoveData.Position) || Vector3.Distance(transform.position, _currentMoveData.Position) <= minDistance)
            {
                _currentMoveDataTimer += Time.deltaTime;
                if (_currentMoveDataTimer > _currentMoveData.Delay)
                {
                    PickNextMoveData();
                }
            }
        }
    }
}
