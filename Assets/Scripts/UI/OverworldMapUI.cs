using System.Collections.Generic;
using Entities.Enemy;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OverworldMapUI : MonoBehaviour
    {
        [System.Serializable]
        public class MapNode
        {
            public Button button;
            public GameObject completedVisual;
            public GameObject currentVisual;
            public GameObject lockedVisual;
            public TextMeshProUGUI nodeLabel;
        }

        [SerializeField] private List<MapNode> nodes;
        [SerializeField] private TextMeshProUGUI currentEnemyText;
        [SerializeField] private Button continueButton;

        private void Start()
        {
            RefreshMap();
        }

        private void RefreshMap()
        {
            if (RunManager.Instance == null)
            {
                Debug.LogWarning("RunManager not found — are you testing directly from Overworld scene?");
                return;
            }

            int currentNode = RunManager.Instance.CurrentNodeIndex;
            

            for (int i = 0; i < nodes.Count; i++)
            {
                MapNode node = nodes[i];
                bool completed = i < currentNode;
                bool current = i == currentNode;
                bool locked = i > currentNode;

                node.completedVisual.SetActive(completed);
                node.currentVisual.SetActive(current);
                node.lockedVisual.SetActive(locked);
                node.button.interactable = current;

                node.button.onClick.RemoveAllListeners();
                if (current)
                    node.button.onClick.AddListener(() => RunManager.Instance.ContinueRun());
            }

            EnemyData nextEnemy = RunManager.Instance.CurrentEnemy;
            if (currentEnemyText != null)
                currentEnemyText.text = "Next: " + nextEnemy.enemyName;
        }

        public void OnContinueClicked()
        {
            RunManager.Instance.ContinueRun();
        }
    }
}