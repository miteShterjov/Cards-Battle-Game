using System;
using Cards;

namespace Events
{
    public static class EnemyEvents
    {
        public static event Action<int> OnEnemyHit;
        public static event Action OnEnemyDeath;
        public static event Action OnEnemyHeal;
        public static event Action<StatusEffectType, int, int> OnApplyStatusEffect;

        public static void EnemyHit(int damage) => OnEnemyHit?.Invoke(damage);
        public static void EnemyDeath() => OnEnemyDeath?.Invoke();
        public static void EnemyHeal() => OnEnemyHeal?.Invoke();
        public static void ApplyStatusEffect(StatusEffectType type, int damage, int duration)
            => OnApplyStatusEffect?.Invoke(type, damage, duration);
    }
}