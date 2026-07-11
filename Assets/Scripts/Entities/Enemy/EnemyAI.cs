using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entities.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private List<EnemyMove> possibleMoves;
        [SerializeField] private int maxRepeatCount = 2;

        private EnemyActionType _lastActionType;
        private int _repeatCount;
        
        public void Initialize(List<EnemyMove> moves, int maxRepeatCount)
        {
            possibleMoves = new List<EnemyMove>(moves);
            this.maxRepeatCount = maxRepeatCount;
            _repeatCount = 0;
            CurrentIntent = null;
        }

        public EnemyMove CurrentIntent { get; private set; }

        public EnemyMove PickNextMove()
        {
            TickCooldowns();

            List<EnemyMove> validMoves = possibleMoves
                .Where(move => move.turnsUntilAvailable <= 0)
                .Where(move => !(move.actionType == _lastActionType && _repeatCount >= maxRepeatCount))
                .ToList();

            if (validMoves.Count == 0)
            {
                validMoves = possibleMoves.Where(move => move.turnsUntilAvailable <= 0).ToList();
            }

            if (validMoves.Count == 0) validMoves = possibleMoves;

            EnemyMove chosen = WeightedRandomPick(validMoves);

            if (chosen.cooldownTurns > 0)
                chosen.turnsUntilAvailable = chosen.cooldownTurns + 1;

            if (chosen.actionType == _lastActionType)
                _repeatCount++;
            else
                _repeatCount = 1;

            _lastActionType = chosen.actionType;
            CurrentIntent = chosen;
            return chosen;
        }

        private void TickCooldowns()
        {
            foreach (EnemyMove move in possibleMoves.Where(move => move.turnsUntilAvailable > 0))
            {
                move.turnsUntilAvailable--;
            }
        }

        private EnemyMove WeightedRandomPick(List<EnemyMove> moves)
        {
            int totalWeight = moves.Sum(m => m.weight);
            int roll = Random.Range(0, totalWeight);

            int cumulative = 0;
            foreach (EnemyMove move in moves)
            {
                cumulative += move.weight;
                if (roll < cumulative) return move;
            }

            return moves[0];
        }
    }
}