using GameplayConfigs;
using UnityEngine;

namespace Prototype.Managers
{
    public class GlobalManager : MonoBehaviour
    {
        public static GlobalManager Instance { get; private set; }

        [SerializeField] private GameValuesSO _gameValuesSo;
        public GameValuesSO GameValuesSo => _gameValuesSo;

        public static bool IsCheatMode
        {
            get
            {
                return Instance.GameValuesSo.GetBool("IS_CHEAT_MODE");
            }
        }
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("GlobalManger instance already initialized!");
            }
        }
    }
}
