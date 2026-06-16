using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
    public class CardData : ScriptableObject
    {
        [Header("Card Info")]
        public CardType type;
        public string cardName;
        public string description;
        public int actionCost;
        public Sprite illustration;

        [Header("Damage")]
        public DamageType damageType = DamageType.Physical;
        public int attackPower;

        [Header("Healing")]
        public int healPower;

        [Header("Defense")]
        public DefenseType defenseType;
        public int defensePower;

        [Header("Status Effect")]
        public StatusEffectType statusEffectType = StatusEffectType.None;
        public int statusEffectDamage;  // for Poison and Burn
        public int statusEffectDuration;
    }

    public enum CardType { Attack, Heal, Defend, Debuff }
    public enum DamageType { Physical, Magical, Pure }
    public enum DefenseType { Armor, Resistance, Buff }
    public enum StatusEffectType { None, Burn, Poison, Stun, Weakness }
}