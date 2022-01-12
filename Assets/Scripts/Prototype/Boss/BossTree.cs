using UnityEngine;

namespace Prototype.Boss
{
    public class BossTree : MonoBehaviour
    {
        [SerializeField] private TargetStepMover _stepMover;
        [SerializeField] private TentacleThroatCollider _throatCollider;

        [Space] 
        [SerializeField] private float _pauseMoveBetweenEatDelay = 1f;
        [SerializeField] private bool _isTreeResetTargetPositionWhenEatTentacle = false;
        
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
        }
    }
}
