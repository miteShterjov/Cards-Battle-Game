using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject resumeButton;
        [SerializeField] private GameObject startButton;

        private void Start()
        {
            bool runInProgress = RunManager.Instance != null && RunManager.Instance.IsRunActive;
            resumeButton.SetActive(runInProgress);
            startButton.SetActive(!runInProgress);
        }

        public void OnStartNewRun()
        {
            RunManager.Instance.StartRun();
        }

        public void OnResumeRun()
        {
            SceneManager.LoadScene("Overworld");
        }
    }
}