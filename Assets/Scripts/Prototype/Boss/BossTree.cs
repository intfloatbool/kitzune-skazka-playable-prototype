using UnityEngine;

namespace Prototype.Boss
{
    public class BossTree : MonoBehaviour
    {
        
        [System.Serializable]
        public class BossTreeBehaviourData
        {
            [Range(0, 50)] public int TentacleCountToSpeedUp;
            [Range(0, 100)] public float SpeedUpBoostPerTentacle;
            [Range(0, 100)] public int MaxSpeedUpAmount;
        }
        
        [SerializeField] private TargetStepMover _stepMover;
        [SerializeField] private TentacleThroatCollider _throatCollider;

        [Space] 
        [SerializeField] private float _pauseMoveBetweenEatDelay = 1f;
        [SerializeField] private bool _isTreeResetTargetPositionWhenEatTentacle = false;
        
        [Space]
        [SerializeField] private BossTreeBehaviourData _behaviourData;

        private int _tentaclesEatenCount;
        private int _speedUpCount;
        
        private void Start()
        {
            _stepMover.ResetMover();
            _stepMover.SetActiveMove(true);
            
            _throatCollider.OnTentacleEatenCallback = OnTentacleEatenCallback;
        }

        private void ResumeMove()
        {
            _stepMover.SetActiveMove(true);
        }

        private void OnTentacleEatenCallback()
        {
            if (_isTreeResetTargetPositionWhenEatTentacle)
            {
                _stepMover.SetCurrentMoveDataIndex(0);   
            }
            _stepMover.SetActiveMove(false);
            Invoke(nameof(ResumeMove), _pauseMoveBetweenEatDelay);
            
            
            foreach (var existedTentacle in _throatCollider.CurrentTentacles)
            {
                if (existedTentacle.CurrentState == Tentacle.TentacleState.ATTACK)
                {
                    existedTentacle.RestMove();
                }
            }
                
            if (_throatCollider.CurrentTentacles.Count <= 0)
            {
                GameManager.Instance.GameWin();
            }
            
            _tentaclesEatenCount++;

            if (_tentaclesEatenCount >= _behaviourData.TentacleCountToSpeedUp)
            {
                if (_speedUpCount <= _behaviourData.MaxSpeedUpAmount)
                {
                    foreach (var moveData in _stepMover.MoveDataCollection)
                    {
                        moveData.Speed += _behaviourData.SpeedUpBoostPerTentacle;
                    }

                    _speedUpCount++;
                }

                _tentaclesEatenCount = 0;
            }
        }
    }
}
