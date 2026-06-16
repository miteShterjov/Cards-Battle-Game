using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;

namespace StatusEffects
{
    public class StatusEffectController : MonoBehaviour
    {
        private readonly List<StatusEffect> _activeEffects = new List<StatusEffect>();
        private HealthController _healthController;

        private void Awake()
        {
            _healthController = GetComponent<HealthController>();
            if (!_healthController) Debug.LogError("HealthController missing on " + gameObject.name);
        }

        public void ApplyEffect(StatusEffect effect)
        {
            // if effect of same type exists, refresh duration instead of stacking
            StatusEffect existing = _activeEffects.FirstOrDefault(e => e.EffectName == effect.EffectName);
            if (existing != null)
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, effect.TurnsRemaining);
            else
                _activeEffects.Add(effect);
        }

        public void ProcessEffects()
        {
            foreach (StatusEffect effect in _activeEffects)
            {
                print($"{effect.EffectName} ticking, turns remaining: {effect.TurnsRemaining}");
                effect.TickEffect(_healthController);
            }

            _activeEffects.RemoveAll(e => e.IsExpired);
        }

        public bool IsStunned => _activeEffects.Any(e => e is StunEffect);
        public bool IsPoisoned => _activeEffects.Any(e => e is PoisonEffect);
        public bool IsWeakened => _activeEffects.Any(e => e is WeaknessEffect);
        public bool IsBurned => _activeEffects.Any(e => e is BurnEffect);
        public float WeaknessMultiplier => IsWeakened ? 
            ((WeaknessEffect)_activeEffects.First(e => e is WeaknessEffect)).DamageMultiplier : 1f;
    }
}