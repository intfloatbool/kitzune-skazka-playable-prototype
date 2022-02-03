using Prototype.Managers;
using UnityEngine;

namespace Prototype.Cheats
{
    public class OnlyCheatGameObject : MonoBehaviour
    {
        private void Start()
        {
            if(!GlobalManager.IsCheatMode)
                gameObject.SetActive(false);
        }

        private void Update()
        {
            if(!GlobalManager.IsCheatMode)
                gameObject.SetActive(false);
        }
    }
}
