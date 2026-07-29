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
        
        public void StartGame() => RunManager.Instance.StartRun();
        public void DeckBuilder() => SceneFader.Instance.FadeToScene("DeckBuilder");
        public void GameStore() => SceneFader.Instance.FadeToScene("GameStore");
        public void MainMenu() => SceneFader.Instance.FadeToScene("MainMenu");

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
