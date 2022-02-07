using System;
using System.Collections.Generic;
using System.Linq;
using BezierSolution;
using Common;
using Prototype.Boss;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Prototype
{
    public class TentaclesController : MonoBehaviour
    {
        [SerializeField] private List<Tentacle> _tentaclesOnScene;
        [SerializeField] private Transform _tentacleAnchorsRoot;
        [SerializeField] private int _countToChangeBehaviour = 3;

        [Space] 
        [SerializeField] private float _attackSpeedIncrease = 2f; 

        [Space]
        [SerializeField] private List<Transform> _rootAnchorsForTentacles;

        private bool _isBehaviourChanged = false;
        private LinkedList<Tentacle> _tempTentacles;
        private Queue<Transform> _tentacleAnchorsQueue;

        private Dictionary<Tentacle, BezierWalkerWithSpeed> _walkersDict =
            new Dictionary<Tentacle, BezierWalkerWithSpeed>();

        private Dictionary<BezierWalkerWithSpeed, float> _walkersBasicSpeedDict =
            new Dictionary<BezierWalkerWithSpeed, float>();

        private TentacleThroatCollider _treeThroat;

        private void OnValidate()
        {
            if (_tentaclesOnScene == null || _tentaclesOnScene.Count <= 0)
            {
                _tentaclesOnScene = FindObjectsOfType<Tentacle>().ToList();
            }
        }

        private void Awake()
        {
            _treeThroat = FindObjectOfType<TentacleThroatCollider>();
            _treeThroat.OnTentacleEatenEv += TreeThroatOnTentacleEatenEv;
        }

        private void OnDestroy()
        {
            if(_treeThroat)
                _treeThroat.OnTentacleEatenEv -= TreeThroatOnTentacleEatenEv;
        }

        private void TreeThroatOnTentacleEatenEv()
        {
            foreach (var tentacle in _tempTentacles)
            {
                var currentMoveDataCollection = tentacle.BodyStepMover.MoveDataCollection.ToList();
                int index = 0;
                foreach (var moveData in currentMoveDataCollection)
                {
                    var clone = moveData.Clone();
                    clone.Speed += _attackSpeedIncrease;
                    tentacle.BodyStepMover.SetMoveData(index, clone);
                    tentacle.SetSpeedInCollection(index, clone.Speed);
                    index++;
                }
                
            }
        }

        private void Start()
        {
            _tempTentacles = new LinkedList<Tentacle>(_tentaclesOnScene);
            foreach (var tentacle in _tempTentacles)
            {
                tentacle.OnKill += TentacleOnKill;
                tentacle.OnStateChangedCallback = OnStateChangedCallback;
            }

            var tentacleAnchors = new List<Transform>(_tentacleAnchorsRoot.childCount);
            foreach (Transform child in _tentacleAnchorsRoot)
            {
                tentacleAnchors.Add(child);
            }

            _tentacleAnchorsQueue = new Queue<Transform>(tentacleAnchors);
        }

        private void OnStateChangedCallback(Tentacle tentacle, Tentacle.TentacleState tentacleState)
        {
            if (_isBehaviourChanged)
            {
                if (tentacleState == Tentacle.TentacleState.ATTACK)
                {
                    tentacle.ResetBodyMoveData();
                    if (_walkersDict.TryGetValue(tentacle, out var walker))
                    {
                        walker.speed = 0;
                        TrySetActiveLookAtComponent(walker, false);
                    }
                    else
                    {
                        Debug.LogError("Walker is missing!");
                    }
                    
                    SetAttackTargetForTentacle(tentacle);
                }

                if (tentacleState == Tentacle.TentacleState.PENDING)
                {
                    if (_walkersDict.TryGetValue(tentacle, out var walker))
                    {
                        walker.speed = _walkersBasicSpeedDict[walker];
                        TrySetActiveLookAtComponent(walker, true);
                    }
                    else
                    {
                        Debug.LogError("Walker is missing!");
                    }
                }
            }
        }

        public void KillRandomTentacle()
        {
            var tentacles = _tempTentacles.ToList();
            if (tentacles.Any())
            {
                var target = tentacles[Random.Range(0, tentacles.Count)];
                target.Kill();
            }
        }

        public void EatRandomTentacle()
        {
            var tentacles = _tempTentacles.ToList();
            if (tentacles.Any())
            {
                var target = tentacles[Random.Range(0, tentacles.Count)];
                if (_treeThroat)
                {
                    _treeThroat.EatTentacle(target);
                }
            } 
        }

        private void TentacleOnKill(Tentacle tentacle)
        {
            _tempTentacles.Remove(tentacle);
            tentacle.OnKill -= TentacleOnKill;

            if (_isBehaviourChanged == false && _tempTentacles.Count <= _countToChangeBehaviour)
            {
                ChangeBehaviour();
                _isBehaviourChanged = true;
            }
        }
        
        private Transform GetAnchorForTentacle(Tentacle tentacle)
        {
            if (!_tentacleAnchorsQueue.Any())
            {
                Debug.LogError("Anchors is ran out!");
                var nullGo = new GameObject("NULL_ANCHOR");
                nullGo.transform.position = Vector3.zero;
                return nullGo.transform;
            }
            Transform nearestAnchor = _tentacleAnchorsQueue.Dequeue();
            return nearestAnchor;
        }

        private void ChangeBehaviour()
        {
            foreach (var tentacle in _tempTentacles)
            {
                Transform nearestAnchor = GetAnchorForTentacle(tentacle);
                nearestAnchor.gameObject.SetActive(true);
                BezierWalkerWithSpeed walker = nearestAnchor.GetComponent<BezierWalkerWithSpeed>();
                if (walker)
                {
                    _walkersDict[tentacle] = walker;
                    _walkersBasicSpeedDict[walker] = walker.speed;
                }
                else
                {
                    Debug.LogError("Walker is missing!");
                }
                
                tentacle.RootStepMover.SetActive(false);

                tentacle.transform.parent = nearestAnchor;
                
                float offset = 5f;
                tentacle.transform.localPosition = Vector3.right * offset;
                tentacle.transform.localRotation = Quaternion.Euler(0,0,90);

                tentacle.ResetBodyMoveData();

                tentacle.OnTentacleActivated += TentacleOnTentacleActivated;
                tentacle.OnTentacleDeactivated += TentacleOnTentacleDeactivated;

                tentacle.IsActiveAutoAttack = true;
            }
        }

        private void SetAttackTargetForTentacle(Tentacle tentacle)
        {
            var currentBodyMoveDataCollection = tentacle.BodyStepMover.MoveDataCollection.ToList();
            var moveDataToTarget = currentBodyMoveDataCollection[1];
            var changedData = moveDataToTarget.Clone();
            var posToAttack = tentacle.TargetTransform.position;
            changedData.Position = tentacle.TentacleBodyTransform.InverseTransformPoint(posToAttack);
            
            tentacle.BodyStepMover.SetMoveData(1, changedData);
        }

        private void TentacleOnTentacleDeactivated(Tentacle tentacle)
        {
            if (_walkersDict.TryGetValue(tentacle, out var walker))
            {
                walker.speed = _walkersBasicSpeedDict[walker];
                TrySetActiveLookAtComponent(walker, true);
            }
            else
            {
                Debug.LogError("Walker is missing!");
            }
        }

        private void TentacleOnTentacleActivated(Tentacle tentacle)
        {
            if (_walkersDict.TryGetValue(tentacle, out var walker))
            {
                walker.speed = 0;
                TrySetActiveLookAtComponent(walker, false);
            }
            else
            {
                Debug.LogError("Walker is missing!");
            }
        }

        private void TrySetActiveLookAtComponent(BezierWalkerWithSpeed walker, bool isActive)
        {
            if (walker.TryGetComponent(out LookAt2DComponent lookAt2DComponent))
            {
                lookAt2DComponent.SetIsEnabled(isActive);                
            }
        }
    }
}
