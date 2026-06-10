using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    public class GameManager : Singleton<GameManager>
    {
        public bool IsGameActive { get; private set; } = true;

        [Header("Game Manager Config")]
        [SerializeField] private float transitionTime = 2f;
        [SerializeField] private GameObject winUI;
        [SerializeField] private GameObject loseUI;

        private void OnEnable()
        {
            EnemyEvents.OnEnemyDeath += PlayerWin;
            PlayerEvents.OnPlayerDeath += PlayerLoose;
        }

        private void OnDisable()
        {
            EnemyEvents.OnEnemyDeath -= PlayerWin;
            PlayerEvents.OnPlayerDeath -= PlayerLoose;
        }

        private void PlayerWin()
        {
            StartCoroutine(RestartGameCo(true));
            IsGameActive = false;
        }
        
        private void PlayerLoose()
        {
            StartCoroutine(RestartGameCo(false));
            IsGameActive = false;
        }
        
        private IEnumerator RestartGameCo(bool win)
        {
            yield return new WaitForSeconds(transitionTime);
            if (win) winUI.SetActive(true);
            else loseUI.SetActive(true);
            
            yield return new WaitForSeconds(transitionTime);
            if (win) winUI.SetActive(false);
            else loseUI.SetActive(false);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}