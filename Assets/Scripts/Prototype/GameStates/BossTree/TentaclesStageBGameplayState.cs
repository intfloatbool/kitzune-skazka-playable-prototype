using DG.Tweening;
using Prototype.Player;
using UnityEngine;

namespace Prototype.GameStates.BossTree
{
    public class TentaclesStageBGameplayState : GameplayStateBase
    {
        private readonly TentaclesController _tentaclesController;
        private readonly Boss.BossTree _bossTree;
        private readonly PlayerController _playerController;
        
        private float _startTime = 2f;
        private float _timerForStart;
        private bool _isDone = false;
        
        public TentaclesStageBGameplayState(TentaclesController tentaclesController)
        {
            this._tentaclesController = tentaclesController;
            
            _bossTree = GameManager.Instance.StaticFindObjectOfType<Boss.BossTree>();
            _playerController = GameManager.Instance.StaticFindObjectOfType<PlayerController>();
        }
        
        public override void OnStateStarted()
        {
            _bossTree.SetActive(false);
            _playerController.SetActive(false);
            _bossTree.ThroatCollider.SetActive(false);
            
            foreach (var kvp in _tentaclesController.BehaviorChangedTentaclesList)
            {
                var anchor = kvp.Item1;
                var tentacle = kvp.Item2;

                tentacle.SetActive(false);
                float offset = 5f;
                var targetAngle = 0f;
                var targetWordPosition = anchor.position + (anchor.right * offset);
                var targetAngles = anchor.GetChild(0).eulerAngles;
                
                tentacle.transform.DOMove(targetWordPosition, _startTime);
                tentacle.transform.DORotate(targetAngles, _startTime);
            }
        }

        public override void Update()
        {
            CalculateTimerLoop();
        }

        private void OnTimerTick()
        {
            _isDone = true;
            ResumeGame();
        }

        private void ResumeGame()
        {
            _bossTree.SetActive(true);
            _playerController.SetActive(true);
            _bossTree.ThroatCollider.SetActive(true);
            
            foreach (var kvp in _tentaclesController.BehaviorChangedTentaclesList)
            {
                var anchor = kvp.Item1;
                var tentacle = kvp.Item2;
                
                anchor.gameObject.SetActive(true);
                tentacle.transform.parent = anchor;
                
                float offset = 5f;
                tentacle.transform.localPosition = Vector3.right * offset;
                tentacle.transform.localRotation = Quaternion.Euler(0,0,90);
                tentacle.ResetBodyMoveData();
                
                tentacle.SetActive(true);
            }
        }

        private void CalculateTimerLoop()
        {
            if(_isDone)
                return;

            if (!_isDone)
            {
                _bossTree.SetActive(false);
            }
            
            if (_timerForStart >= _startTime)
            {
                OnTimerTick();
            }
            
            _timerForStart += Time.deltaTime;
        }

        public override void OnStateStopped()
        {
            
        }
    }
}