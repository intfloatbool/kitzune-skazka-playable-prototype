using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Prototype
{
    [RequireComponent(typeof(Button))]
    public class ReloadCurrentSceneButton : MonoBehaviour
    {
        private void Awake()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}