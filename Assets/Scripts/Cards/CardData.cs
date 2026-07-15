using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
    public class CardData : ScriptableObject
    {
        [Header("Card Info")]
        public string cardId;
        public CardType type;
        public string cardName;
        public string description;
        public int actionCost;
        public int goldCost;
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
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(cardId) && !string.IsNullOrEmpty(cardName))
            {
                cardId = cardName.ToLower().Replace(" ", "_");
            }
        }
        #endif
    }

    public enum CardType { Attack, Spell, Heal, Defend, Debuff }
    public enum DamageType { Physical, Magical, Pure }
    public enum DefenseType { Armor, Resistance, Buff }
    public enum StatusEffectType { None, Burn, Poison, Stun, Weakness }
}