using System;
using Prototype.Player;
using UnityEngine;

namespace Prototype.Boss
{
    public class Tentacle : MonoBehaviour
    {
        [SerializeField] private TriggerCollider _activateTrigger;
        [SerializeField] private TriggerCollider _killTrigger;
        [SerializeField] private TargetStepMover _targetStepMover;

        [SerializeField] private Transform _tentacleBodyTransform;
        [SerializeField] private Transform _targetTransform;
        
        [Space]
        [SerializeField] private float[] _delaysCollection;
        [SerializeField] private float[] _speedsCollection;
        
        private void Awake()
        {
            _activateTrigger.OnTriggerCallback = OnActivateTriggerEnter;
            _killTrigger.OnTriggerCallback = OnKillTriggerEnter;
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
        }

        private void OnActivateTriggerEnter(TriggerableObject triggerable, Collider2D col)
        {
            var player = triggerable.GetComponent<FoxPlayer>();
            if (player)
            {
                _targetStepMover.ResetMover();
                _targetStepMover.SetActiveMove(true);
            }
        }
        
        private void OnKillTriggerEnter(TriggerableObject triggerable, Collider2D col)
        {
            var player = triggerable.GetComponent<FoxPlayer>();
            if (player)
            {
                
            }
        }
        
    }
}
