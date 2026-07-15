using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;

namespace StatusEffects
{
    public class StatusEffectController : MonoBehaviour
    {
        public event Action OnDeathFromStatusEffect;

        private readonly List<StatusEffect> _activeEffects = new List<StatusEffect>();
        private HealthController _healthController;
        private bool _hasDied;

        private void Awake()
        {
            _healthController = GetComponent<HealthController>();
            if (!_healthController) Debug.LogError("HealthController missing on " + gameObject.name);
        }

        public void ApplyEffect(StatusEffect effect)
        {
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
                if (_hasDied) break;

                print($"{effect.EffectName} ticking, turns remaining: {effect.TurnsRemaining}");
                effect.TickEffect(_healthController);

                if (!_healthController.IsAlive() && !_hasDied)
                {
                    _hasDied = true;
                    OnDeathFromStatusEffect?.Invoke();
                }
            }

            _activeEffects.RemoveAll(e => e.IsExpired);
        }
        
        public void ResetEffects()
        {
            _activeEffects.Clear();
            _hasDied = false;
        }

        public bool IsStunned => _activeEffects.Any(e => e is StunEffect);
        public bool IsPoisoned => _activeEffects.Any(e => e is PoisonEffect);
        public bool IsWeakened => _activeEffects.Any(e => e is WeaknessEffect);
        public bool IsBurned => _activeEffects.Any(e => e is BurnEffect);
        public float WeaknessMultiplier => IsWeakened ? 
            ((WeaknessEffect)_activeEffects.First(e => e is WeaknessEffect)).DamageMultiplier : 1f;
    }
}