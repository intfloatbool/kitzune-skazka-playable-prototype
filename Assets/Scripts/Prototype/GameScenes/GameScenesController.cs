using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.GameScenes
{
    public class GameScenesController : MonoBehaviour
    {
        public static GameScenesController Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;   
            }
            else
            {
                Debug.LogError("Some instance already initialized!");
            }
        }

        public void LoadSceneByName(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
