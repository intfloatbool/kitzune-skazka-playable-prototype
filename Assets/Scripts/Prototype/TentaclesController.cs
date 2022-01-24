using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private float[] _delaysCollection;
        [SerializeField] private float[] _speedsCollection;
        [SerializeField] private float[] _offsetsCollection;
        
        private bool _isBehaviourChanged = false;
        private LinkedList<Tentacle> _tempTentacles;
        private Queue<Transform> _tentacleAnchorsQueue;
        private void OnValidate()
        {
            if (_tentaclesOnScene == null || _tentaclesOnScene.Count <= 0)
            {
                _tentaclesOnScene = FindObjectsOfType<Tentacle>().ToList();
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
                    tentacle.ResetMoveData();
                    tentacle.RootStepMover.SetActiveMove(false);
                    SetAttackTargetForTentacle(tentacle);
                }

                if (tentacleState == Tentacle.TentacleState.PENDING)
                {
                    tentacle.RootStepMover.SetActiveMove(true);
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

                tentacle.transform.position = nearestAnchor.position;
                tentacle.transform.rotation = nearestAnchor.rotation;

                tentacle.ResetMoveData();
                var topPos = tentacle.transform.position + Vector3.up * _offsetsCollection[0];
                var bottomPos = tentacle.transform.position + Vector3.up * _offsetsCollection[1];
                
                tentacle.RootStepMover.AddMoveData(new TargetStepMover.MoveData()
                {
                    Delay = _delaysCollection[0],
                    Position = topPos,
                    Speed = _speedsCollection[0]
                });
                
                tentacle.RootStepMover.AddMoveData(new TargetStepMover.MoveData()
                {
                    Delay = _delaysCollection[1],
                    Position = bottomPos,
                    Speed = _speedsCollection[1]
                });
                
                tentacle.RootStepMover.ResetMover();
                tentacle.RootStepMover.SetActiveMove(true);
                
                tentacle.OnTentacleActivated += TentacleOnTentacleActivated;
                tentacle.OnTentacleDeactivated += TentacleOnTentacleDeactivated;

                //tentacle.OnTriggeredCallback = OnTentacleTriggered;
            }
        }

        private void SetAttackTargetForTentacle(Tentacle tentacle)
        {
            var currentBodyMoveDataCollection = tentacle.BodyStepMover.MoveDataCollection.ToList();
            var moveDataToTarget = currentBodyMoveDataCollection[1];
            var changedData = moveDataToTarget.Clone();
            var posToAttack = tentacle.TentacleBodyTransform.position;
            posToAttack.x = tentacle.TargetTransform.position.x;
            changedData.Position = tentacle.TentacleBodyTransform.InverseTransformPoint(posToAttack);
            
            tentacle.BodyStepMover.SetMoveData(1, changedData);
        }

        private void TentacleOnTentacleDeactivated(Tentacle tentacle)
        {
            tentacle.RootStepMover.SetActiveMove(true);
        }

        private void TentacleOnTentacleActivated(Tentacle tentacle)
        {
            tentacle.RootStepMover.SetActiveMove(false);
        }
    }
}
