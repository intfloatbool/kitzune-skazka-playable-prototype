using System;
using System.Collections;
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

        [SerializeField] private UnityEvent _onGameStartedEv;
        public event Action OnGameStarted;
        
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

        [SerializeField] private DynamicObjectsController _dynamicObjectsController;
        [SerializeField] private float _delayBeforeLose = 2f;

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

        private void Start()
        {
            GameHelper.SetTimeScale(1f);
            
            _onGameStartedEv?.Invoke();
            OnGameStarted?.Invoke();
        }

        public void DoGameAction(GameAction gameAction)
        {
            switch (_currentGameState)
            {
                case GameState.GAMEPLAY:
                {
                    if (gameAction == GameAction.TO_WIN)
                    {
                        SetState(GameState.WIN);
                    }
                    else if (gameAction == GameAction.TO_LOSE)
                    {
                        SetState(GameState.LOSE);
                    }
                    break;
                }
                case GameState.LOSE:
                {
                    break;
                }
                case GameState.WIN:
                {
                    break;
                }
            }
        }

        public void SetState(GameState state)
        {
            if (state != _currentGameState)
            {
                _currentGameState = state;
                OnGameStateChanged?.Invoke(state);
                Debug.Log("GameState changed: " + state);
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
            DoGameAction(GameAction.TO_WIN);
            OnGameWin?.Invoke();
            _onGameWinEv?.Invoke();
        }

        public void GameLose()
        {
            Invoke(nameof(GameLoseDelayed), _delayBeforeLose);
        }

        private void GameLoseDelayed()
        {
            DoGameAction(GameAction.TO_LOSE);
            OnGameLose?.Invoke();
            _onGameLoseEv?.Invoke();
        }

        public void PauseGame()
        {
            _dynamicObjectsController.SetActiveAll(false);
        }

        public void GameResume()
        {
            GameHelper.SetTimeScale(1f);
        }

        private void OnDestroy()
        {
            if(Instance)
                Instance = null;
            
            FoxPlayer.OnPlayerCreated -= FoxPlayerOnOnPlayerCreated;
        }

        public T StaticFindObjectOfType<T>() where T : Component
        {
            return FindObjectOfType<T>();
        }

        public void StartCustomCoroutine(IEnumerator coroutine, Action onDone = null)
        {
            IEnumerator temp()
            {
                yield return StartCoroutine(coroutine);
                onDone?.Invoke();
            }
            
            StartCoroutine(temp());
        }
    }
}
