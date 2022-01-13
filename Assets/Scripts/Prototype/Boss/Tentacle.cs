using System;
using System.Collections;
using Prototype.Player;
using UnityEngine;

namespace Prototype.Boss
{
    public class Tentacle : MonoBehaviour
    {
        public enum TentacleState
        {
            NONE = 0,
            PENDING = 1,
            ATTACK = 2,
            RETURN = 3
        }
        
        [SerializeField] private TriggerCollider _activateTrigger;
        [SerializeField] private TargetStepMover _bodyStepMover;
        [SerializeField] private TargetStepMover _rootStepMover;
        public TargetStepMover RootStepMover => _rootStepMover;

        [SerializeField] private Transform _tentacleBodyTransform;
        [SerializeField] private Transform _targetTransform;

        [Space] 
        [SerializeField] private float _activationDelay = 1f;
        [SerializeField] private float[] _delaysCollection;
        [SerializeField] private float[] _speedsCollection;

        private Coroutine _activationCoroutine;

        public event Action OnTentacleActivated;
        public event Action OnTentacleDeactivated;

        [Space] 
        [Header("Runtime")] 
        [SerializeField] private TentacleState _currentState;

        public event Action<Tentacle> OnKill; 

        public TentacleState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
            }
        }

        private Vector3 _bodyLocalPositionAtStart;
        
        private void Awake()
        {
            _activateTrigger.OnTriggerCallback = OnActivateTriggerEnter;

            _bodyLocalPositionAtStart = _tentacleBodyTransform.localPosition;
        }

        public void RestMove()
        {
            _currentState = TentacleState.RETURN;
            _bodyStepMover.SetCurrentMoveDataIndex(2);
        }

        public void Kill()
        {
            OnKill?.Invoke(this);
            Destroy(gameObject);
        }

        private void Start()
        {
            CurrentState = TentacleState.PENDING;
            ResetMoveData();
        }

        public void ResetMoveData()
        {
            Vector3 baseBodyLocalPosition = _bodyLocalPositionAtStart;
            _tentacleBodyTransform.localPosition = baseBodyLocalPosition;
            _bodyStepMover.ClearMoveData();
            _bodyStepMover.SetIsLocal(true);
            _bodyStepMover.AddMoveData(new TargetStepMover.MoveData()
            {
                Position = baseBodyLocalPosition,
                Delay = _delaysCollection[0],
                Speed = _speedsCollection[0]
            });
            
            _bodyStepMover.AddMoveData(new TargetStepMover.MoveData()
            {
                Position = _tentacleBodyTransform.InverseTransformPoint(_targetTransform.position),
                Delay = _delaysCollection[1],
                Speed = _speedsCollection[1]
            });
            
            _bodyStepMover.AddMoveData(new TargetStepMover.MoveData()
            {
                Position = baseBodyLocalPosition,
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

            CurrentState = TentacleState.ATTACK;
            
            bool isProcessDone = false;
            _bodyStepMover.OnLoopDoneCallback = () =>
            {
                isProcessDone = true;
            };
            _bodyStepMover.ResetMover();
            _bodyStepMover.SetActiveMove(true);

            yield return new WaitForEndOfFrame();
            
            while (!isProcessDone)
            {
                if (Vector3.Distance(_bodyStepMover.transform.position, _targetTransform.position) <= 0.03f)
                {
                    CurrentState = TentacleState.RETURN;
                }

                yield return null;
            }

            _bodyStepMover.SetActiveMove(false);
            
            _activationCoroutine = null;
            OnTentacleDeactivated?.Invoke();
            
            CurrentState = TentacleState.PENDING;
            yield return null;
        }
    }
}
