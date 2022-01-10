using System;
using System.Collections;
using Prototype.Player;
using UnityEngine;

namespace Prototype.Boss
{
    public class Tentacle : MonoBehaviour
    {
        [SerializeField] private TriggerCollider _activateTrigger;
        [SerializeField] private TargetStepMover _targetStepMover;

        [SerializeField] private Transform _tentacleBodyTransform;
        [SerializeField] private Transform _targetTransform;

        [Space] 
        [SerializeField] private float _activationDelay = 1f;
        [SerializeField] private float[] _delaysCollection;
        [SerializeField] private float[] _speedsCollection;

        private Coroutine _activationCoroutine;

        public event Action OnTentacleActivated;
        public event Action OnTentacleDeactivated;
        
        private void Awake()
        {
            _activateTrigger.OnTriggerCallback = OnActivateTriggerEnter;
        }

        private void Start()
        {
            var basePosition = _tentacleBodyTransform.position;
            _targetStepMover.AddMoveData(new TargetStepMover.MoveData()
            {
               Position = basePosition,
               Delay = _delaysCollection[0],
               Speed = _speedsCollection[0]
            });
            
            _targetStepMover.AddMoveData(new TargetStepMover.MoveData()
            {
                Position = _targetTransform.position,
                Delay = _delaysCollection[1],
                Speed = _speedsCollection[1]
            });
            
            _targetStepMover.AddMoveData(new TargetStepMover.MoveData()
            {
                Position = basePosition,
                Delay = _delaysCollection[0],
                Speed = _speedsCollection[0]
            });
        }

        private void OnActivateTriggerEnter(TriggerableObject triggerable, Collider2D col)
        {
            var player = triggerable.GetComponent<FoxPlayer>();
            if (player)
            {
                bool isTentacleActivated = _activationCoroutine != null;
                if (!isTentacleActivated)
                {
                    _activationCoroutine = StartCoroutine(TentacleProcessCoroutine());
                }
            }
        }

        private IEnumerator TentacleProcessCoroutine()
        {
            OnTentacleActivated?.Invoke();
            yield return new WaitForSeconds(_activationDelay);

            bool isProcessDone = false;
            _targetStepMover.OnLoopDoneCallback = () =>
            {
                isProcessDone = true;
            };
            _targetStepMover.ResetMover();
            _targetStepMover.SetActiveMove(true);

            while (!isProcessDone)
            {
                yield return null;
            }
            
            _targetStepMover.SetActiveMove(false);
            
            _activationCoroutine = null;
            OnTentacleDeactivated?.Invoke();
            yield return null;
        }
    }
}
