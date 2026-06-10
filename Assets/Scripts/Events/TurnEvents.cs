using UnityEngine;
using System;

namespace Events
{
    public class TurnEvents
    {
        public static event Action OnPlayerTurnEnds;
        public static event Action OnEnemyTurnEnds;
        public static event Action OnEnemyTurnStart;
        public static event Action OnPlayerTurnStart;

        public static void PlayerTurnEnds() => OnPlayerTurnEnds?.Invoke();
        public static void EnemyTurnEnds() => OnEnemyTurnEnds?.Invoke();
        public static void EnemyTurnStart() => OnEnemyTurnStart?.Invoke();
        public static void PlayerTurnStart() => OnPlayerTurnStart?.Invoke();
    }
}
