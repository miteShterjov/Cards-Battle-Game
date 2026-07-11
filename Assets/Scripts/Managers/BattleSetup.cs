using Entities.Enemy;
using Managers;
using UnityEngine;

public class BattleSetup : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private SpriteRenderer backgroundRenderer;

    private void Start()
    {
        Debug.Log($"BattleSetup — RunManager null: {RunManager.Instance == null}");
        if (RunManager.Instance == null) return;
    
        Debug.Log($"IsRunActive: {RunManager.Instance.IsRunActive}, Node: {RunManager.Instance.CurrentNodeIndex}, Enemy: {RunManager.Instance.CurrentEnemy?.enemyName}");
    
        if (!RunManager.Instance.IsRunActive) return;

        enemyController.SetEnemyData(RunManager.Instance.CurrentEnemy);

        if (backgroundRenderer != null && RunManager.Instance.CurrentBackground != null)
            backgroundRenderer.sprite = RunManager.Instance.CurrentBackground;
    }
}