using System;
using Cards;
using Events;
using StatusEffects;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerController : CombatantController
    {
        [Header("Heal Visual Config")]
        [SerializeField] private GameObject healVisualPrefab;
        [SerializeField] private float healVisualScale = 0.6f;

        private PlayerHealthController _playerHealthController;
        private StatusEffectController _statusEffectController;

        protected override void Awake()
        {
            base.Awake();
            _playerHealthController = GetComponent<PlayerHealthController>();
            if (!_playerHealthController) Debug.LogError("PlayerHealthController missing.");
            _statusEffectController = GetComponent<StatusEffectController>();
            if (!_statusEffectController) Debug.LogError("StatusEffectController missing.");
        }

        private void OnEnable()
        {
            PlayerEvents.OnCardPlayed += HandleCardPlayed;
            PlayerEvents.OnPlayerHit += PlayerHit;
            TurnEvents.OnPlayerTurnStart += ProcessStatusEffects;
        }

        private void OnDisable()
        {
            PlayerEvents.OnCardPlayed -= HandleCardPlayed;
            PlayerEvents.OnPlayerHit -= PlayerHit;
            TurnEvents.OnPlayerTurnStart -= ProcessStatusEffects;
        }
        
        private void ProcessStatusEffects() => _statusEffectController.ProcessEffects();

        private void HandleCardPlayed(CardData cardData)
        {
            switch (cardData.type)
            {
                case CardType.Attack:
                    int damage = Mathf.RoundToInt(cardData.attackPower * _statusEffectController.WeaknessMultiplier);
                    StartCoroutine(AttackMoveCo(new Vector3(4f, 0, 0), 
                        () => EnemyEvents.EnemyHit(damage)));
                    break;
                case CardType.Heal:
                    Heal(cardData);
                    break;
                case CardType.Defend:
                    _playerHealthController.AddArmor(cardData.defensePower);
                    break;
                case CardType.Debuff:
                    ApplyStatusEffectToEnemy(cardData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Heal(CardData cardData)
        {
            _playerHealthController.HealDamage(cardData.healPower);
            CastHealVisualEffect();
        }

        private void PlayerHit(int damage)
        {
            _playerHealthController.TakeDamage(damage);
            if (_playerHealthController.IsAlive()) TakeHitAnimEvent();
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

        private void CastHealVisualEffect()
        {
            GameObject healVisual = Instantiate(healVisualPrefab, transform.position, Quaternion.identity, transform);
            healVisual.transform.localScale = Vector3.one * healVisualScale;
        }
    }
}