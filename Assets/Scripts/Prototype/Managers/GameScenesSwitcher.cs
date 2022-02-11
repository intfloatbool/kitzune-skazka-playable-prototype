using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Prototype.Managers
{
    public class GameScenesSwitcher : MonoBehaviour
    {
        public static GameScenesSwitcher Instance { get; private set; }
        
        [SerializeField] private GameObject _loadingRoot;
        [SerializeField] private Image _fadingImage;
        [SerializeField] private float _delayBeforeLoad = 0.5f;
        [SerializeField] private float _fadeTime = 0.5f;
        [SerializeField] private Color _fadeColor = Color.black;

        private Coroutine _loadingCoroutine;

        public static void LoadCustomScene(string sceneName)
        {
            if (Instance)
            {
                Instance.LoadScene(sceneName);
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
                Debug.LogError("GameScenesSwitcher instance already instantiated!");
            }
        }

        public void LoadScene(string sceneName)
        {
            if (_loadingCoroutine != null)
            {
                Debug.LogError("Some scene in progress.");
                return;
            }
        
            _loadingCoroutine = StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            _loadingRoot.SetActive(true);
            var hideColor = _fadeColor;
            hideColor.a = 0f;
            _fadingImage.color = hideColor;

            var tween = _fadingImage.DOColor(_fadeColor, _fadeTime);
            yield return new WaitForSeconds(_delayBeforeLoad);
            var asyncOp = SceneManager.LoadSceneAsync(sceneName);
            asyncOp.allowSceneActivation = true;
            
            while (!asyncOp.isDone)
            {
                
                yield return null;
            }

            while (asyncOp.progress < 1f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(_delayBeforeLoad);
            _fadingImage.color = _fadeColor;
            yield return new WaitForEndOfFrame();
            tween = _fadingImage.DOColor(hideColor, _fadeTime);
            tween.onComplete = () =>
            {
                _loadingRoot.SetActive(false);
                tween.Kill();
            };
            _loadingCoroutine = null;
        }
    }
}