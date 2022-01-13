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
        private List<Transform> _tentacleAnchors;
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
            }

            _tentacleAnchors = new List<Transform>(_tentacleAnchorsRoot.childCount);
            foreach (Transform child in _tentacleAnchorsRoot)
            {
                _tentacleAnchors.Add(child);
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

        private void ChangeBehaviour()
        {
            foreach (var tentacle in _tempTentacles)
            {
                Transform nearestAnchor = null;

                float nearestDistance = Mathf.Infinity;
                foreach (var anchor in _tentacleAnchors)
                {
                    var distance = Vector3.Distance(anchor.position, tentacle.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestAnchor = anchor;
                        nearestDistance = distance;
                    }
                }

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
                
            }
        }
    }
}
