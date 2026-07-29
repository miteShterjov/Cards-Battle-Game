using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class SceneFader : Singleton<SceneFader>
    {
        [SerializeField] private Image fadePanel;
        [SerializeField] private float fadeDuration = 0.5f;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            StartCoroutine(FadeIn());
        }

        public IEnumerator FadeIn()
        {
            fadePanel.gameObject.SetActive(true);
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadePanel.color = new Color(0, 0, 0, 1f - (elapsed / fadeDuration));
                yield return null;
            }
            fadePanel.color = new Color(0, 0, 0, 0);
            fadePanel.gameObject.SetActive(false);
        }

        public IEnumerator FadeOut()
        {
            fadePanel.gameObject.SetActive(true);
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadePanel.color = new Color(0, 0, 0, elapsed / fadeDuration);
                yield return null;
            }
            fadePanel.color = new Color(0, 0, 0, 1);
        }

        public void FadeToScene(string sceneName)
        {
            StartCoroutine(FadeToSceneCo(sceneName));
        }

        private IEnumerator FadeToSceneCo(string sceneName)
        {
            yield return StartCoroutine(FadeOut());

            var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone) yield return null;

            yield return null;

            yield return StartCoroutine(FadeIn());
        }
    }
}