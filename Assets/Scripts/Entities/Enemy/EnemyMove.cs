using Cards;
using UnityEngine;

namespace Entities.Enemy
{
    public enum EnemyActionType { Attack, Defend, Debuff, Spell, Heal }

    [System.Serializable]
    public class EnemyMove
    {
        public string moveName;
        public EnemyActionType actionType;
        [Range(0, 100)] public int weight;

        [Header("Cooldown")]
        [Tooltip("Turns to wait before this move can be picked again. 0 = no cooldown.")]
        public int cooldownTurns;
        [HideInInspector] public int turnsUntilAvailable; // runtime tracking, not set in Inspector

        [Header("Attack / Spell")]
        public int damage;
        public DamageType damageType = DamageType.Physical;

        [Header("Defend")]
        public int armorValue;

        [Header("Heal")]
        public int healAmount;

        [Header("Debuff")]
        public StatusEffectType statusEffectType;
        public int statusEffectDamage;
        public int statusEffectDuration;
    }
}