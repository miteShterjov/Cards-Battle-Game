using Entities.Enemy;
using StatusEffects;
using UnityEngine;

namespace Managers
{
    public class BattleSetup : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyController;
        [SerializeField] private GameObject backgroundRenderer;
        

        private void Start()
        {
            Debug.Log($"BattleSetup — RunManager null: {RunManager.Instance == null}");
            if (RunManager.Instance == null) return;
    
            Debug.Log($"IsRunActive: {RunManager.Instance.IsRunActive}, Node: {RunManager.Instance.CurrentNodeIndex}, Enemy: {RunManager.Instance.CurrentEnemy?.enemyName}");
    
            if (!RunManager.Instance.IsRunActive) return;

            enemyController.SetEnemyData(RunManager.Instance.CurrentEnemy);

            GenerateSceneBackground();

            enemyController.GetComponent<StatusEffectController>()?.ResetEffects();
        }

        private void GenerateSceneBackground()
        {
            if (backgroundRenderer == null || RunManager.Instance.CurrentBackground == null) return;
            
            GameObject backgroundPrefab = RunManager.Instance.CurrentBackground;

            GameObject imageBackground = Instantiate(backgroundPrefab, backgroundRenderer.transform);
            imageBackground.transform.position = GetTransformPosition(backgroundPrefab);
        }

        private static Vector3 GetTransformPosition(GameObject backgroundPrefab)
        {
            return backgroundPrefab.name switch
            {
                "cellar_background" => new Vector3(0, 0, 0),
                "forest_background" => new Vector3(1.2f, 4.8f, 0),
                "graveyard_background" => new Vector3(0.18f, 0.2f, 0),
                "tavern_background" => new Vector3(0.4f, 0.6f, 0),
                _ => Vector3.zero
            };
        }
    }
}