using Cards;
using Events;
using StatusEffects;
using TMPro;
using UnityEngine;

namespace Entities.Enemy
{
    public class EnemyController : CombatantController
    {
        [Header("Enemy Config")]
        [SerializeField] private float takeHitAnimDelay = 0.25f;
        [SerializeField] private float deathAnimDelay = 0.25f;
        [SerializeField] private Vector3 attackMovement = new Vector3(-2f, 0, 0);

        [Header("Telegraph Moves")]
        [SerializeField] private EnemyAI enemyAI;
        [SerializeField] private TextMeshProUGUI intentText;

        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            EnemyEvents.OnEnemyHit += HandleEnemyHit;
            EnemyEvents.OnApplyStatusEffect += HandleStatusEffectApplied;

            TurnEvents.OnPlayerTurnStart += TelegraphNextMove;
            TurnEvents.OnEnemyTurnStart += ExecuteTelegraphedMove;
            TurnEvents.OnEnemyTurnStart += ProcessStatusEffects;
        }

        private void OnDisable()
        {
            EnemyEvents.OnEnemyHit -= HandleEnemyHit;
            EnemyEvents.OnApplyStatusEffect -= HandleStatusEffectApplied;

            TurnEvents.OnPlayerTurnStart -= TelegraphNextMove;
            TurnEvents.OnEnemyTurnStart -= ExecuteTelegraphedMove;
            TurnEvents.OnEnemyTurnStart -= ProcessStatusEffects;
        }
        
        public void SetEnemyData(EnemyData data)
        {
            _animator.runtimeAnimatorController = data.animatorController;
            HealthController.SetMaxHealth(data.maxHealth);
            enemyAI.Initialize(data.moves, data.maxRepeatCount);
            if (intentText != null) intentText.text = "";
        }
        
        protected override void HandleDeathFromStatusEffect()
        {
            base.HandleDeathFromStatusEffect(); // plays death anim
            EnemyEvents.EnemyDeath();
        }

        private void ProcessStatusEffects() => StatusEffectController.ProcessEffects();

        private void HandleEnemyHit(int damage)
        {
            HealthController.TakeDamage(damage);
            if (HealthController.IsAlive()) Invoke(nameof(TakeHitAnimEvent), takeHitAnimDelay);
            else
            {
                Invoke(nameof(DeathAnimEvent), deathAnimDelay);
                EnemyEvents.EnemyDeath();
            }
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

        private void TelegraphNextMove()
        {
            if (StatusEffectController.IsStunned)
            {
                intentText.text = "Enemy is Stunned";
                return;
            }

            EnemyMove move = enemyAI.PickNextMove();
            intentText.text = "Enemy will: " + move.actionType;
        }

        private void ExecuteTelegraphedMove()
        {
            if (StatusEffectController.IsStunned)
            {
                print("Enemy is stunned, skipping turn");
                return;
            }

            EnemyMove move = enemyAI.CurrentIntent;
            if (move == null) return;

            switch (move.actionType)
            {
                case EnemyActionType.Attack:
                case EnemyActionType.Spell:
                    StartCoroutine(AttackMoveCo(attackMovement,
                        () => PlayerEvents.PlayerHit(move.damage, move.damageType)));
                    break;
                case EnemyActionType.Defend:
                    HealthController.AddArmor(move.armorValue);
                    break;
                case EnemyActionType.Debuff:
                    PlayerEvents.ApplyStatusEffect(move.statusEffectType, move.statusEffectDamage, move.statusEffectDuration);
                    break;
                case EnemyActionType.Heal: 
                    Heal(move.healAmount); 
                    EnemyEvents.EnemyHeal();
                    break;
            }
        }
    }
}