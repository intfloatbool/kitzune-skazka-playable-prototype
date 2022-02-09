using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prototype.GameStates
{
    public class GameStatesController : MonoBehaviour
    {
        public static GameStatesController Instance { get; private set; }
        
        private GameplayStateBase _currentState = new NullGameplayState();

        [System.Serializable]
        private class StateNameParams
        {
            public string stateName;
            public KVP[] parameters;
        }

        [System.Serializable]
        private class KVP
        {
            public string key;
            public string value;
        }

        [SerializeField] private StateNameParams[] _specificParams;
        
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

            var specificParamsForState =
                _specificParams.FirstOrDefault(p => p.stateName.Equals(state.GetType().Name));
            if (specificParamsForState != null)
            {
                var paramsDict = new Dictionary<string, string>(specificParamsForState.parameters.Length);
                foreach (var param in specificParamsForState.parameters)
                {
                    paramsDict.Add(param.key, param.value);
                }
                
                _currentState.SetParams(paramsDict);
            }
            
            
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
