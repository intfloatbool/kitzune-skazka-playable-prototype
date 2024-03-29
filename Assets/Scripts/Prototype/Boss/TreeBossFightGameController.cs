﻿using System.Collections.Generic;
using Prototype.GameStates;
using Prototype.GameStates.BossTree;
using Prototype.Managers;
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

            gameObject.AddComponent<AudioSource>();
            
            SoundManager.PlayMusic("music_tree_boss_stage_1_loop", gameObject, true);
        }

        private void InitStatesQueue()
        {
            _statesQueue = new Queue<GameplayStateBase>(new GameplayStateBase[]
            {
                new NullGameplayState(),
                new TentaclesStageBGameplayState(this, _tentaclesController)
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