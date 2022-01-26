using System;
using System.Collections;
using Prototype.Player;
using UnityEngine;
using Random = UnityEngine.Random;

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
        public TargetStepMover BodyStepMover => _bodyStepMover;
        
        [SerializeField] private TargetStepMover _rootStepMover;
        public TargetStepMover RootStepMover => _rootStepMover;

        [SerializeField] private Transform _tentacleBodyTransform;
        public Transform TentacleBodyTransform => _tentacleBodyTransform;
        
        [SerializeField] private Transform _targetTransform;
        public Transform TargetTransform => _targetTransform;

        [Space] 
        [SerializeField] private float _activationDelay = 1f;
        [SerializeField] private float[] _delaysCollection;
        [SerializeField] private float[] _speedsCollection;

        private Coroutine _activationCoroutine;

        public event Action<Tentacle> OnTentacleActivated;
        public event Action<Tentacle> OnTentacleDeactivated;

        [SerializeField] private float _autoattackTimeSecondsMax = 3f;
        [SerializeField] private float _autoattackTimeSecondsMin = 0.5f;

        private float _autoattackTimeSeconds;
        
        [Space] 
        [Header("Runtime")] 
        [SerializeField] private TentacleState _currentState;
        

        public event Action<Tentacle> OnKill;
        public Action<Tentacle> OnTriggeredCallback { get; set; } 

        public TentacleState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                OnStateChangedCallback?.Invoke(this, _currentState);
            }
        }

        public Action<Tentacle, TentacleState> OnStateChangedCallback { get; set; } 

        private Vector3 _bodyLocalPositionAtStart;

        private float _autoattackTimer;

        public bool IsActiveAutoAttack { get; set; } = false;

        private BossTree _bossTree;
        
        private void Awake()
        {
            _activateTrigger.OnTriggerCallback = OnActivateTriggerEnter;

            _bodyLocalPositionAtStart = _tentacleBodyTransform.localPosition;

            _autoattackTimeSeconds = Random.Range(_autoattackTimeSecondsMin, _autoattackTimeSecondsMax);

            _bossTree = FindObjectOfType<BossTree>();
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
            ResetBodyMoveData();
        }

        public void ResetBodyMoveData()
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
                StartAttack();
            }
        }

        public void StartAttack()
        {
            if(_bossTree && _bossTree.IsBossStopped)
                return;
            
            bool isTentacleActivated = _activationCoroutine != null;
            if (!isTentacleActivated)
            {
                _activationCoroutine = StartCoroutine(TentacleProcessCoroutine());
                OnTriggeredCallback?.Invoke(this);
            } 
        }

        private IEnumerator TentacleProcessCoroutine()
        {
            OnTentacleActivated?.Invoke(this);
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
                if (_bossTree && _bossTree.IsBossStopped)
                {
                    RestMove();
                }
                
                if (Vector3.Distance(_bodyStepMover.transform.position, _targetTransform.position) <= 0.03f)
                {
                    CurrentState = TentacleState.RETURN;
                }

                yield return null;
            }

            _bodyStepMover.SetActiveMove(false);

            _autoattackTimer = 0f;
            _activationCoroutine = null;
            OnTentacleDeactivated?.Invoke(this);
            
            CurrentState = TentacleState.PENDING;
            yield return null;
        }

        private void Update()
        {
            if(IsActiveAutoAttack)
                AutoAttackLoop();
        }

        private void AutoAttackLoop()
        {
            if(_activationCoroutine != null)
                return;
            
            if (_autoattackTimer > _autoattackTimeSeconds)
            {
                 StartAttack();
                 _autoattackTimer = 0f;
                 _autoattackTimeSeconds = Random.Range(_autoattackTimeSecondsMin, _autoattackTimeSecondsMax);
            }

            _autoattackTimer += Time.deltaTime;
        }
    }
}
