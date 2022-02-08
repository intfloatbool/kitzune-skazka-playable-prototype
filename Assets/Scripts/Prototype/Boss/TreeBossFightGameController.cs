using System.Collections.Generic;
using Prototype.GameStates;
using Prototype.GameStates.BossTree;
using UnityEngine;

namespace Prototype.Boss
{
    public class TreeBossFightGameController: MonoBehaviour
    {
        [SerializeField] private TentaclesController _tentaclesController;
        [SerializeField] private DynamicObjectsController _dynamicObjectsController;
        
        private GameStatesController _statesController;
        private Queue<GameplayStateBase> _statesQueue;

        private void Start()
        {
            _statesController = GameStatesController.Instance;
            if (!_statesController)
            {
                Debug.LogError("States controller is missing!");
            }

            InitStatesQueue();
        }

        private void InitStatesQueue()
        {
            _statesQueue = new Queue<GameplayStateBase>(new GameplayStateBase[]
            {
                new NullGameplayState(),
                new TentaclesStageBGameplayState(_tentaclesController)
            });  
        }

        public void RunNextState()
        {
            if (_statesQueue.Count <= 0)
            {
                Debug.LogError("There is no states!");
                return;
            } 
            
            _statesController.SetState(_statesQueue.Dequeue());
        }
    }
}