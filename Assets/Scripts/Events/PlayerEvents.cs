using System;
using Cards;

namespace Events
{
    public static class PlayerEvents
    {
        public static event Action<CardData> OnCardPlayed;
        public static event Action OnPlayerDeath;
        public static event Action OnDrawCardRequested;
        public static event Action OnReshuffleRequested;
        public static event Action OnPlayerHealed;
        public static event Action OnDrawCardSucceeded;
        public static event Action<int, DamageType> OnPlayerHit; // ← now carries damage type
        public static event Action<StatusEffectType, int, int> OnApplyStatusEffect; // ← new

        public static void CardPlayed(CardData cardData) => OnCardPlayed?.Invoke(cardData);
        public static void PlayerDeath() => OnPlayerDeath?.Invoke();
        public static void DrawCardRequested() => OnDrawCardRequested?.Invoke();
        public static void ReshuffleRequested() => OnReshuffleRequested?.Invoke();
        public static void PlayerHealed() => OnPlayerHealed?.Invoke();
        public static void DrawCardSucceeded() => OnDrawCardSucceeded?.Invoke();
        public static void PlayerHit(int damage, DamageType type = DamageType.Physical) => OnPlayerHit?.Invoke(damage, type);
        public static void ApplyStatusEffect(StatusEffectType type, int damage, int duration) => OnApplyStatusEffect?.Invoke(type, damage, duration);
    }
}
