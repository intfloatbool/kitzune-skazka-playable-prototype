using System;
using System.Collections.Generic;
using System.Linq;
using Prototype.Boss;
using UnityEngine;

namespace Prototype
{
    public class TentaclesController : MonoBehaviour
    {
        [SerializeField] private List<Tentacle> _tentaclesOnScene;
        [SerializeField] private int _countToChangeBehaviour = 3;
        [SerializeField] private float[] _delaysCollection;
        [SerializeField] private float[] _speedsCollection;
        [SerializeField] private float[] _offsetsCollection;
        
        private bool _isBehaviourChanged = false;
        private LinkedList<Tentacle> _tempTentacles;
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
                var topPos = tentacle.BasePosition + Vector3.up * _offsetsCollection[0];
                var bottomPos = tentacle.BasePosition + Vector3.up * _offsetsCollection[1];
                
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
