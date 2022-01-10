using UnityEngine;

namespace Prototype
{
    public class GameStateGameObject : MonoBehaviour
    {
        [SerializeField] private GameState _necessaryState;
        [SerializeField] private GameObject[] _childs;

        private void Start()
        {
            if (GameManager.Instance)
            {
                OnGameStateChanged(GameManager.Instance.CurrentGameState);
                GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
            }
            else
            {
                Debug.LogError("GameManager is missing!!");
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            }
        }

        private void OnGameStateChanged(GameState state)
        {
            foreach (var child in _childs)
            {
                if(child)
                    child.SetActive(state == _necessaryState);
            }
            
        }
    }
}