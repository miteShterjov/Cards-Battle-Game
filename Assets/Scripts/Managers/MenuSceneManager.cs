using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MenuSceneManager : MonoBehaviour
    {
        private string[] _scenes =
        {
            "MainMenu@0",
            "DeckBuilder@1",
            "GameScene@2",
            "GameStore@3",
            "Overworld@4"
        };
        
        public void StartGame() => SceneManager.LoadScene(4);
        public void DeckBuilder() => SceneManager.LoadScene(1);
        public void GameStore() => SceneManager.LoadScene(3);
        public void MainMenu() => SceneManager.LoadScene(0);

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
