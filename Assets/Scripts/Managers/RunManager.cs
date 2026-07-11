using System.Collections.Generic;
using Entities.Enemy;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class RunManager : Singleton<RunManager>
    {
        [Header("Run Config")]
        [SerializeField] private List<EnemyData> enemySequence;
        [SerializeField] private List<Sprite> nodeBackgrounds;
        [SerializeField] private string battleSceneName = "GameScene";
        [SerializeField] private string overworldSceneName = "Overworld";
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        public int CurrentNodeIndex => PlayerDataManager.Instance.CurrentData.currentNodeIndex;
        public bool IsRunActive => PlayerDataManager.Instance.CurrentData.runActive;
        public bool IsLastNode => CurrentNodeIndex >= enemySequence.Count - 1;
        public int TotalNodes => enemySequence.Count;
        public EnemyData CurrentEnemy => enemySequence[CurrentNodeIndex];
        public Sprite CurrentBackground => nodeBackgrounds.Count > CurrentNodeIndex
            ? nodeBackgrounds[CurrentNodeIndex]
            : null;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void StartRun()
        {
            PlayerDataManager.Instance.CurrentData.currentNodeIndex = 0;
            PlayerDataManager.Instance.CurrentData.runActive = true;
            PlayerDataManager.Instance.SaveGame();
            SceneManager.LoadScene(overworldSceneName);
        }

        public void ContinueRun()
        {
            LoadBattleScene();
        }

        public void NodeCompleted()
        {
            if (IsLastNode)
            {
                RunCompleted();
                return;
            }

            PlayerDataManager.Instance.CurrentData.currentNodeIndex++;
            PlayerDataManager.Instance.SaveGame();
            SceneManager.LoadScene(overworldSceneName);
        }

        public void RunCompleted()
        {
            PlayerDataManager.Instance.CurrentData.runActive = false;
            PlayerDataManager.Instance.CurrentData.currentNodeIndex = 0;
            PlayerDataManager.Instance.AddGold(100);
            PlayerDataManager.Instance.SaveGame();
            SceneManager.LoadScene(mainMenuSceneName);
        }

        public void RunFailed()
        {
            PlayerDataManager.Instance.CurrentData.runActive = false;
            PlayerDataManager.Instance.CurrentData.currentNodeIndex = 0;
            PlayerDataManager.Instance.SaveGame();
            SceneManager.LoadScene(mainMenuSceneName);
        }

        private void LoadBattleScene()
        {
            SceneManager.LoadScene(battleSceneName);
        }
    }
}