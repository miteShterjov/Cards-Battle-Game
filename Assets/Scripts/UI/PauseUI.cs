using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        // [SerializeField] private GameObject otherToDisable;
        // [SerializeField] private GameObject otherToDisable2;

        // private void Start()
        // {
        //     if (SceneManager.GetActiveScene().name != "GameSceneScene") return;
        //     otherToDisable2 = null;
        // }

        private void Update()
        {
            if (!Keyboard.current.escapeKey.wasPressedThisFrame) return;
            if (pauseMenu.activeSelf) ResumeGame();
            else pauseMenu.SetActive(true);
        //     otherToDisable.SetActive(!pauseMenu.activeSelf);
        //     if (SceneManager.GetActiveScene().name == "GameSceneScene") return;
        //     otherToDisable2.SetActive(!pauseMenu.activeSelf);
        }

        public void ResumeGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        public void MainMenu() => SceneManager.LoadScene(0);
        public void DeckBuilder() => SceneManager.LoadScene(1);
        //public void GameStore() => SceneManager.LoadScene(4);
        public void NewGame() => SceneManager.LoadScene(2);

        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
