using Cards;
using Events;
using StatusEffects;
using UnityEngine;

namespace Entities.Enemy
{
    public class EnemyController : CombatantController
    {
        [Header("Enemy Config")]
        [SerializeField] private int attackDamage = 10;
        [SerializeField] private float takeHitAnimDelay = 0.25f;
        [SerializeField] private float deathAnimDelay = 0.25f;
        [SerializeField] private Vector3 attackMovement = new Vector3(-2f, 0, 0);

        private EnemyHealthController _enemyHealthController;
        private StatusEffectController _statusEffectController;

        

        protected override void Awake()
        {
            base.Awake();
            _enemyHealthController = GetComponent<EnemyHealthController>();
            if (!_enemyHealthController) Debug.LogError("EnemyHealthController missing.");
            _statusEffectController = GetComponent<StatusEffectController>();
            if (!_statusEffectController) Debug.LogError("StatusEffectController missing.");
        }

        private void Update()
        {
            Animator.SetBool(StunnedAnimEvent, _statusEffectController.IsStunned);
            SpriteRenderer.color = GetStatusColor();
        }

        private void OnEnable()
        {
            EnemyEvents.OnEnemyHit += HandleEnemyHit;
            EnemyEvents.OnApplyStatusEffect += HandleStatusEffectApplied;

            TurnEvents.OnEnemyTurnStart += Attack;
            TurnEvents.OnEnemyTurnStart += ProcessStatusEffects;
        }

        private void OnDisable()
        {
            EnemyEvents.OnEnemyHit -= HandleEnemyHit;
            EnemyEvents.OnApplyStatusEffect -= HandleStatusEffectApplied;

            TurnEvents.OnEnemyTurnStart -= Attack;
            TurnEvents.OnEnemyTurnStart -= ProcessStatusEffects;
        }
        
        private void ProcessStatusEffects() => _statusEffectController.ProcessEffects();

        private void HandleEnemyHit(int damage)
        {
            _enemyHealthController.TakeDamage(damage);
            if (_enemyHealthController.IsAlive()) Invoke(nameof(TakeHitAnimEvent), takeHitAnimDelay);
            else
            {
                Invoke(nameof(DeathAnimEvent), deathAnimDelay);
                EnemyEvents.EnemyDeath();
            }
        }

        private void Attack()
        {
            if (_statusEffectController.IsStunned)
            {
                print("Enemy is stunned, skipping turn");
                return;
            }

            StartCoroutine(AttackMoveCo(attackMovement, () => PlayerEvents.PlayerHit(attackDamage)));
        }
        
        private void HandleStatusEffectApplied(StatusEffectType type, int damage, int duration)
        {
            print($"Enemy received status effect: {type}, damage: {damage}, duration: {duration}");
            StatusEffect effect = type switch
            {
                StatusEffectType.Poison   => new PoisonEffect(damage, duration),
                StatusEffectType.Burn     => new BurnEffect(damage, duration),
                StatusEffectType.Stun     => new StunEffect(duration),
                StatusEffectType.Weakness => new WeaknessEffect(duration),
                _ => null
            };

            if (effect != null) _statusEffectController.ApplyEffect(effect);
        }
        
        private Color GetStatusColor()
        {
            if (_statusEffectController.IsBurned)    return new Color(1f, 0.3f, 0f);      // orange-red
            if (_statusEffectController.IsPoisoned)  return new Color(0f, 0.5f, 0f);      // dark green
            if (_statusEffectController.IsWeakened)  return new Color(0.6f, 0.6f, 0.6f); // grey
            return Color.white; // no effect
        }
    }
}