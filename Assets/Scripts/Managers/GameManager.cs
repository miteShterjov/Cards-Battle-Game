using System.Collections;
using Events;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public bool IsGameActive { get; private set; } = true;

        [Header("Game Manager Config")]
        [SerializeField] private float transitionTime = 2f;
        [SerializeField] private GameObject winUI;
        [SerializeField] private GameObject loseUI;
        [Header("Gold Rewards")]
        [SerializeField] private int winGoldReward = 4;
        [SerializeField] private int lossGoldReward = 1;

        private void OnEnable()
        {
            EnemyEvents.OnEnemyDeath += HandlePlayerWin;
            PlayerEvents.OnPlayerDeath += HandlePlayerLoss;
        }

        private void OnDisable()
        {
            EnemyEvents.OnEnemyDeath -= HandlePlayerWin;
            PlayerEvents.OnPlayerDeath -= HandlePlayerLoss;
        }
        
        private void HandlePlayerWin() => EndGameSequence(true);
        private void HandlePlayerLoss() => EndGameSequence(false);


        private void EndGameSequence(bool win)
        {
            PlayerDataManager.Instance.AddGold(win ? winGoldReward : lossGoldReward);
            
            if (win) PlayerDataManager.Instance.CurrentData.wins++;
            else PlayerDataManager.Instance.CurrentData.losses++;
            
            PlayerDataManager.Instance.SaveGame();
            
            StartCoroutine(RestartGameCo(win));
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