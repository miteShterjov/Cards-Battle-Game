using System.Collections.Generic;
using UnityEngine;

namespace Entities.Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Header("Enemy Info")]
        public string enemyName;
        public int maxHealth;
        public RuntimeAnimatorController animatorController;
        
        [Header("Spawn Config")]
        public Vector3 spawnOffset;

        [Header("AI Config")]
        public List<EnemyMove> moves;
        public int maxRepeatCount = 2;
    }
}