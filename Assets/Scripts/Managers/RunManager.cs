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
        [SerializeField] private List<GameObject> nodeBackgrounds;
        [SerializeField] private string battleSceneName = "GameScene";
        [SerializeField] private string overworldSceneName = "Overworld";
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        [SerializeField] private string endCreditsSceneName = "EndCredits";

        public int CurrentNodeIndex => PlayerDataManager.Instance.CurrentData.currentNodeIndex;
        public bool IsRunActive => PlayerDataManager.Instance.CurrentData.runActive;
        public bool IsLastNode => CurrentNodeIndex >= enemySequence.Count - 1;
        public int TotalNodes => enemySequence.Count;
        public EnemyData CurrentEnemy => enemySequence[CurrentNodeIndex];
        public GameObject CurrentBackground => nodeBackgrounds[Random.Range(0, nodeBackgrounds.Count)];

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
            SceneFader.Instance.FadeToScene(overworldSceneName);
        }

        public void ContinueRun()
        {
            SceneFader.Instance.FadeToScene(battleSceneName);
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
            SceneFader.Instance.FadeToScene(overworldSceneName);
        }

        public void RunCompleted()
        {
            PlayerDataManager.Instance.CurrentData.runActive = false;
            PlayerDataManager.Instance.CurrentData.currentNodeIndex = 0;
            PlayerDataManager.Instance.AddGold(100);
            PlayerDataManager.Instance.SaveGame();
            SceneFader.Instance.FadeToScene(endCreditsSceneName);
        }

        public void RunFailed()
        {
            PlayerDataManager.Instance.CurrentData.runActive = false;
            PlayerDataManager.Instance.CurrentData.currentNodeIndex = 0;
            PlayerDataManager.Instance.SaveGame();
            SceneFader.Instance.FadeToScene(mainMenuSceneName);
        }

        private void LoadBattleScene()
        {
            SceneFader.Instance.FadeToScene(battleSceneName);
        }
    }
}