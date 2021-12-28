using GameplayConfigs;
using UnityEngine;

namespace Prototype
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static IGameValuesProvider ValuesProvider => Instance?.GameValuesProvider;
        
        [SerializeField] private GameValuesSO _gameValuesSo;

        public IGameValuesProvider GameValuesProvider => _gameValuesSo;

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
        }

        private void OnDestroy()
        {
            if(Instance)
                Instance = null;
        }
    }
}
