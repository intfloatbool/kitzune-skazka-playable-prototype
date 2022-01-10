using System;
using GameplayConfigs;
using Prototype.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Prototype
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static IGameValuesProvider ValuesProvider => Instance?.GameValuesProvider;
        
        [SerializeField] private GameValuesSO _gameValuesSo;

        public IGameValuesProvider GameValuesProvider => _gameValuesSo;

        private FoxPlayer _lastPlayerInstance;

        [SerializeField] private UnityEvent _onPlayerDeadEv;
        public event Action OnPlayerDeadEv;
        
        [SerializeField] private UnityEvent _onGameWinEv;
        public event Action OnGameWin;
        
        [SerializeField] private UnityEvent _onGameLoseEv;
        public event Action OnGameLose;

        private GameState _currentGameState;
        public GameState CurrentGameState => _currentGameState;
        public event Action<GameState> OnGameStateChanged;

        [Space]
        [SerializeField] private GameState _startGameState;
        

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("Some instance already initialized!");
            }
            
            FoxPlayer.OnPlayerCreated += FoxPlayerOnOnPlayerCreated;
            
            SetState(_startGameState);
        }

        public void SetState(GameState state)
        {
            if (state != _currentGameState)
            {
                _currentGameState = state;
                OnGameStateChanged?.Invoke(state);
            }
        }

        private void FoxPlayerOnOnPlayerCreated(FoxPlayer player)
        {
            if (_lastPlayerInstance)
                _lastPlayerInstance.OnDead -= OnPlayerDead;
            
            player.OnDead += OnPlayerDead;
            _lastPlayerInstance = player;
        }

        private void OnPlayerDead()
        {
            OnPlayerDeadEv?.Invoke();
            _onPlayerDeadEv?.Invoke();
        }

        public void GameWin()
        {
            SetState(GameState.WIN);
            OnGameWin?.Invoke();
            _onGameWinEv?.Invoke();
        }

        public void GameLose()
        {
            SetState(GameState.LOSE);
            OnGameLose?.Invoke();
            _onGameLoseEv?.Invoke();
        }

        private void OnDestroy()
        {
            if(Instance)
                Instance = null;
            
            FoxPlayer.OnPlayerCreated -= FoxPlayerOnOnPlayerCreated;
        }
    }
}
