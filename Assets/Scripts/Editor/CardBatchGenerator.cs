// Editor-only utility script — batch creates CardData ScriptableObjects.
// Place this file anywhere under an "Editor" folder in your Unity project
// (e.g. Assets/Scripts/Editor/CardBatchGenerator.cs), then in Unity's menu bar:
// Tools → Generate New Cards
//
// After running once, DELETE this script or move it out of the project —
// it's a one-time generator, not something that should ship or run repeatedly.

using System.IO;
using Cards;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class CardBatchGenerator
    {
        private const string OutputFolder = "Assets/ScriptableObjects"; // adjust path to match your project structure

        [MenuItem("Tools/Generate New Cards")]
        public static void GenerateCards()
        {
            if (!Directory.Exists(OutputFolder))
                Directory.CreateDirectory(OutputFolder);

            CreateCard("Strike", CardType.Attack, 1, attackPower: 6, damageType: DamageType.Physical,
                description: "A basic, reliable strike.");

            CreateCard("Heavy Slash", CardType.Attack, 7, attackPower: 12, damageType: DamageType.Physical,
                description: "A powerful but costly swing.");

            CreateCard("Arcane Bolt", CardType.Attack, 5, attackPower: 10, damageType: DamageType.Magical,
                description: "A bolt of raw magic that ignores armor.");

            CreateCard("Quick Jab", CardType.Attack, 2, attackPower: 4, damageType: DamageType.Physical,
                description: "A fast, cheap strike.");

            CreateCard("Soul Spike", CardType.Attack, 7, attackPower: 10, damageType: DamageType.Magical,
                description: "A devastating magical strike. Bypasses armor.");

            CreateCard("Guard", CardType.Defend, 2, defensePower: 5, defenseType: DefenseType.Armor,
                description: "Raise your guard.");

            CreateCard("Bulwark", CardType.Defend, 7, defensePower: 12, defenseType: DefenseType.Armor,
                description: "A heavy defensive stance.");

            CreateCard("Iron Stance", CardType.Defend, 2, defensePower: 6, defenseType: DefenseType.Armor,
                description: "A steady defensive posture.");

            CreateCard("Mend", CardType.Heal, 1, healPower: 5,
                description: "A minor restorative effect.");

            CreateCard("Second Wind", CardType.Heal, 5, healPower: 10,
                description: "Catch your breath and recover.");

            CreateCard("Renewal", CardType.Heal, 7, healPower: 12,
                description: "A powerful restorative surge.");

            CreateCard("Poison Dart", CardType.Debuff, 1, statusEffectType: StatusEffectType.Poison,
                statusEffectDamage: 3, statusEffectDuration: 3,
                description: "Inflicts poison, dealing damage over time.");

            CreateCard("Ember Touch", CardType.Debuff, 2, statusEffectType: StatusEffectType.Burn,
                statusEffectDamage: 4, statusEffectDuration: 2,
                description: "Sets the target ablaze, bypassing armor.");

            CreateCard("Crippling Blow", CardType.Debuff, 2, statusEffectType: StatusEffectType.Weakness,
                statusEffectDuration: 2,
                description: "Weakens the target's offense.");

            CreateCard("Paralyze", CardType.Debuff, 3, statusEffectType: StatusEffectType.Stun,
                statusEffectDuration: 1,
                description: "Stuns the target, forcing them to skip their next turn.");

            CreateCard("Venom Strike", CardType.Debuff, 2, statusEffectType: StatusEffectType.Poison,
                statusEffectDamage: 4, statusEffectDuration: 2,
                description: "A poisoned strike that lingers.");

            CreateCard("Mind Spike", CardType.Debuff, 2, statusEffectType: StatusEffectType.Weakness,
                statusEffectDuration: 3,
                description: "A prolonged weakening effect.");

            CreateCard("Wildfire", CardType.Debuff, 3, statusEffectType: StatusEffectType.Burn,
                statusEffectDamage: 5, statusEffectDuration: 3,
                description: "A raging fire that burns for a long duration.");

            CreateCard("Toxic Cloud", CardType.Debuff, 2, statusEffectType: StatusEffectType.Poison,
                statusEffectDamage: 5, statusEffectDuration: 2,
                description: "A cloud of toxic gas.");

            CreateCard("Numbing Strike", CardType.Debuff, 1, statusEffectType: StatusEffectType.Weakness,
                statusEffectDuration: 1,
                description: "A quick strike that briefly weakens the target.");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Card generation complete. Check: " + OutputFolder);
        }

        private static void CreateCard(
            string name,
            CardType type,
            int actionCost,
            int attackPower = 0,
            int healPower = 0,
            int defensePower = 0,
            DamageType damageType = DamageType.Physical,
            DefenseType defenseType = DefenseType.Armor,
            StatusEffectType statusEffectType = StatusEffectType.None,
            int statusEffectDamage = 0,
            int statusEffectDuration = 0,
            string description = "")
        {
            CardData card = ScriptableObject.CreateInstance<CardData>();
            card.cardName = name;
            card.cardId = name.ToLower().Replace(" ", "_");
            card.type = type;
            card.actionCost = actionCost;
            card.attackPower = attackPower;
            card.healPower = healPower;
            card.defensePower = defensePower;
            card.damageType = damageType;
            card.defenseType = defenseType;
            card.statusEffectType = statusEffectType;
            card.statusEffectDamage = statusEffectDamage;
            card.statusEffectDuration = statusEffectDuration;
            card.description = description;

            string path = $"{OutputFolder}/{card.cardId}.asset";
            AssetDatabase.CreateAsset(card, path);
        }
    }
}