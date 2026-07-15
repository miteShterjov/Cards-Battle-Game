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
            "Press Escape to enable the pause menu or again to resume",
            "The Deck Builder is the place where you can create your deck",
            "Buy new cards or card backs from the store.",
            "Stun is a very powerful effect that can be applied to enemies, buy the card first.",
            "Player skin coming to store soon!",
            "Magic damage ignores armor. It's good thing to know."
        };
        
        private void Update()
        {
            if (!Keyboard.current.escapeKey.wasPressedThisFrame) return;
            if (pauseMenu.activeSelf) ResumeGame();
            else
            {
                pauseMenu.SetActive(true);
                DisplayHelpHin();
            };
        }

        private static void ResumeGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        public void MainMenu() => SceneManager.LoadScene(0);
        public void DeckBuilder() => SceneManager.LoadScene(1);
        public void GameStore() => SceneManager.LoadScene(4);
        public void NewGame() => SceneManager.LoadScene(2);

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
