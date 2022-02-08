using UnityEngine;

namespace Prototype.GameStates
{
    public class GameStatesController : MonoBehaviour
    {
        public static GameStatesController Instance { get; private set; }
        
        private GameplayStateBase _currentState = new NullGameplayState();

        [Space]
        [Header("Runtime")]
        [SerializeField] private string _lastStateName;
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void SetState(GameplayStateBase state)
        {
            _currentState.OnStateStopped();
            _currentState = state;
            _currentState.OnStateStarted();
            _lastStateName = state.GetType().FullName;
            Debug.Log($"[GameStatesController] state changed to: {_lastStateName}");
        }

        private void Update()
        {
            _currentState.Update();
        }
    }
}
