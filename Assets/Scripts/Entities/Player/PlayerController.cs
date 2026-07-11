using System;
using Cards;
using Events;
using StatusEffects;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerController : CombatantController
    {
        [Header("Enemy Reference")]
        // will need rework when implementing multiple enemies logic later on
        [SerializeField] private StatusEffectController enemyStatusEffectController;

        private void OnEnable()
        {
            PlayerEvents.OnCardPlayed += HandleCardPlayed;
            PlayerEvents.OnPlayerHit += PlayerHit;
            PlayerEvents.OnApplyStatusEffect += HandleStatusEffectApplied;
            TurnEvents.OnPlayerTurnStart += ProcessStatusEffects;
        }

        private void OnDisable()
        {
            PlayerEvents.OnCardPlayed -= HandleCardPlayed;
            PlayerEvents.OnPlayerHit -= PlayerHit;
            PlayerEvents.OnApplyStatusEffect -= HandleStatusEffectApplied;
            TurnEvents.OnPlayerTurnStart -= ProcessStatusEffects;
        }
        
        protected override void HandleDeathFromStatusEffect()
        {
            base.HandleDeathFromStatusEffect();
            PlayerEvents.PlayerDeath();
        }

        private void ProcessStatusEffects() => StatusEffectController.ProcessEffects();

        private void HandleCardPlayed(CardData cardData)
        {
            switch (cardData.type)
            {
                case CardType.Attack:
                    int damage = Mathf.RoundToInt(cardData.attackPower * enemyStatusEffectController.WeaknessMultiplier);
                    StartCoroutine(AttackMoveCo(new Vector3(4f, 0, 0), 
                        () => EnemyEvents.EnemyHit(damage)));
                    break;
                case CardType.Heal:
                    Heal(cardData.healPower); 
                    PlayerEvents.PlayerHealed();
                    break;
                case CardType.Defend:
                    HealthController.AddArmor(cardData.defensePower);
                    break;
                case CardType.Debuff:
                    ApplyStatusEffectToEnemy(cardData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayerHit(int damage, DamageType type)
        {
            HealthController.TakeDamage(damage, type);
            if (HealthController.IsAlive()) TakeHitAnimEvent();
            else
            {
                DeathAnimEvent();
                PlayerEvents.PlayerDeath();
            }
        }
        
        private void ApplyStatusEffectToEnemy(CardData cardData)
        {
            if (cardData.statusEffectType == StatusEffectType.None) return;
            print($"Applying {cardData.statusEffectType} to enemy");
            EnemyEvents.ApplyStatusEffect
            (cardData.statusEffectType, 
             cardData.statusEffectDamage, 
             cardData.statusEffectDuration);
        }
        
        private void HandleStatusEffectApplied(StatusEffectType type, int damage, int duration)
        {
            StatusEffect effect = type switch
            {
                StatusEffectType.Poison   => new PoisonEffect(damage, duration),
                StatusEffectType.Burn     => new BurnEffect(damage, duration),
                StatusEffectType.Stun     => new StunEffect(duration),
                StatusEffectType.Weakness => new WeaknessEffect(duration),
                _ => null
            };

            if (effect != null) StatusEffectController.ApplyEffect(effect);
        }
    }
}