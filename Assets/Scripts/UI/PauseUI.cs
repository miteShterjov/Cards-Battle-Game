using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private TextMeshProUGUI helpHintsText;

        private string[] _helpHints = new[]
        {
            "Press Space on  to enable the pause menu or again to resume",
            "The Deck Builder is the place where you can create your deck",
            "Buy new cards or card backs from the store.",
            "Stun is a very powerful effect that can be applied to enemies, buy the card first.",
            "Player skin coming to store soon!",
            "Magic damage ignores armor. It's good thing to know."
        };
        
        private void Update()
        {
            if (!Keyboard.current.spaceKey.wasPressedThisFrame) return;
            HandlePauseMenu();
        }

        public void HandlePauseMenu()
        {
            if (pauseMenu.activeSelf) pauseMenu.SetActive(false);
            else
            {
                pauseMenu.SetActive(true);
                DisplayHelpHin();
            }
        }

        public void MainMenu() => SceneFader.Instance.FadeToScene("MainMenu");
        public void DeckBuilder() => SceneFader.Instance.FadeToScene("DeckBuilder");
        public void GameStore() => SceneFader.Instance.FadeToScene("GameStore");
        // public void NewGame() => SceneManager.LoadScene(2);

        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        public void DisplayHelpHin()
        {
            helpHintsText.text = _helpHints[Random.Range(0, _helpHints.Length)];
        }
    }
}
