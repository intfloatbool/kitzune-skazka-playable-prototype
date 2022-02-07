using UnityEngine;

namespace Prototype.Boss
{
    public class BossTree : DynamicGameObject
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

        public bool IsBossStopped { get; private set; }

        [SerializeField] private Animator[] _animators;
        
        private void Start()
        {
            _stepMover.ResetMover();
            _stepMover.SetActive(true);
            
            _throatCollider.OnTentacleEatenCallback = OnTentacleEatenCallback;
        }

        private void ResumeMove()
        {
            _stepMover.SetActive(true);
            IsBossStopped = false;
        }

        private void OnTentacleEatenCallback()
        {
            if (_isTreeResetTargetPositionWhenEatTentacle)
            {
                _stepMover.SetCurrentMoveDataIndex(0);   
            }
            _stepMover.SetActive(false);
            IsBossStopped = true;
            Invoke(nameof(ResumeMove), _pauseMoveBetweenEatDelay);
            
            
            foreach (var existedTentacle in _throatCollider.CurrentTentacles)
            {
                if (existedTentacle.CurrentState == Tentacle.TentacleState.ATTACK || existedTentacle.CurrentState == Tentacle.TentacleState.PENDING)
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

        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            foreach (var animator in _animators)
            {
                if (animator)
                {
                    animator.enabled = isActive;   
                }
            }
        }
    }
}
