using System;

public static class EnemyEvents
{
    public static event Action<CardData> OnEnemyHit;
    public static event Action OnEnemyDeath;

    public static void EnemyHit(CardData cardData) => OnEnemyHit?.Invoke(cardData);
    public static void EnemyDeath() => OnEnemyDeath?.Invoke();
}
