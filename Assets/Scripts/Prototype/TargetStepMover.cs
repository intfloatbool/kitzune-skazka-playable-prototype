using UnityEngine;

namespace Prototype
{
    public class TargetStepMover : MonoBehaviour
    {
        [System.Serializable]
        private struct MoveData
        {
            public Vector3 Position;
            public float Speed;
            public float Delay;
        }
        [SerializeField] private int _startMoveIndex;
        [SerializeField] private MoveData[] _moveDataCollection;
        
        private MoveData _currentMoveData;
        private int _currentMoveDataIndex;

        private float _currentMoveDataTimer;

        private bool _isActive;
        
        public void SetActiveMove(bool isActive)
        {
            _isActive = isActive;
        }

        public void Reset()
        {
            _currentMoveDataIndex = _startMoveIndex;
            SelectMoveData(); 
        }

        private void PickNextMoveData()
        {
            IncreaseNextDataIndex();
            SelectMoveData();
        } 

        private void IncreaseNextDataIndex()
        {
            _currentMoveDataIndex++;
            if (_currentMoveDataIndex > _moveDataCollection.Length - 1)
            {
                _currentMoveDataIndex = 0;
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

            if (transform.position.Equals(_currentMoveData.Position))
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
