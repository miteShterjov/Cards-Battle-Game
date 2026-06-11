using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    public class MenuSceneManager : MonoBehaviour
    {
        public void StartGame() => SceneManager.LoadScene(2);
        public void DeckBuilder() => SceneManager.LoadScene(1);
        public void GameStore() => SceneManager.LoadScene(4);

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
