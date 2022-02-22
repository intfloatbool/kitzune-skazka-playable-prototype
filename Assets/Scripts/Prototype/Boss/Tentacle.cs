using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Prototype.Managers;
using Prototype.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Prototype.Boss
{
    public class Tentacle : DynamicGameObject
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

        [SerializeField] private List<Animator> _animators;

        private readonly string isAttackAnimationKey = "isAttack";
        private readonly string triggerAnimationTriggerKey = "trigger";
        private readonly string speedAnimationKey = "speed";
        private readonly string attackAnimationKey = "Attack";

        public event Action<Tentacle> OnKill;
        public Action<Tentacle> OnTriggeredCallback { get; set; }

        [SerializeField] private Transform _killTriggerTransform;

        private Vector3 _basicKillTrigerLocalPos;

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

        public string AttackSoundName { get; set; } = string.Empty;

        private void Awake()
        {
            _activateTrigger.OnTriggerCallback = OnActivateTriggerEnter;

            _bodyLocalPositionAtStart = _tentacleBodyTransform.localPosition;

            _autoattackTimeSeconds = Random.Range(_autoattackTimeSecondsMin, _autoattackTimeSecondsMax);

            _bossTree = FindObjectOfType<BossTree>();

            _basicKillTrigerLocalPos = _killTriggerTransform.localPosition;

            gameObject.AddComponent<AudioSource>();
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

        public void SetSpeedInCollection(int index, float speed)
        {
            if(index > _speedsCollection.Length - 1)
                return;
            
            _speedsCollection[index] = speed;
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
            if (!_isActive)
            {
                return;
            }
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
            _animators.ForEach(a =>
            {
                a.SetTrigger(triggerAnimationTriggerKey);
                a.SetBool(isAttackAnimationKey, true);
            });

            var newKillerTriggerPos = Vector3.up * 2.2f;
            _killTriggerTransform.DOLocalMove(newKillerTriggerPos, _activationDelay);
            
            yield return new WaitForSeconds(_activationDelay / 2f);
            
            if (!string.IsNullOrEmpty(AttackSoundName))
            {
                SoundManager.PlaySound(AttackSoundName, gameObject);
            }
            
            yield return new WaitForSeconds(_activationDelay / 2f);

            CurrentState = TentacleState.ATTACK;
            
            bool isProcessDone = false;
            _bodyStepMover.OnLoopDoneCallback = () =>
            {
                isProcessDone = true;
            };
            _bodyStepMover.ResetMover();
            _bodyStepMover.SetActive(true);

            yield return new WaitForEndOfFrame();

            Vector3 endPointForKillerTriggerLocalPos = _basicKillTrigerLocalPos;
            endPointForKillerTriggerLocalPos.y = 0f;
            
            
            while (!isProcessDone)
            {
                _killTriggerTransform.localPosition = Vector3.MoveTowards(_killTriggerTransform.localPosition,
                    endPointForKillerTriggerLocalPos, 10 * Time.deltaTime);
                
                if (!_isActive)
                {
                    break;
                }
                
                if (_bossTree && _bossTree.IsBossStopped)
                {
                    _animators.ForEach(a =>
                    {
                        a.SetBool(isAttackAnimationKey, false);
                    });
                    
                    RestMove();
                }
                
                if (Vector3.Distance(_bodyStepMover.transform.position, _targetTransform.position) <= 0.03f)
                {
                    CurrentState = TentacleState.RETURN;
                }

                yield return null;
            }

            _killTriggerTransform.localPosition = _basicKillTrigerLocalPos;

            _animators.ForEach(a =>
            {
                a.SetBool(isAttackAnimationKey, false);
            });
            
            _bodyStepMover.SetActive(false);

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
