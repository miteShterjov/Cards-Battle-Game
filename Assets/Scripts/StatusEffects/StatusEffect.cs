using Entities;

namespace StatusEffects
{
    public abstract class StatusEffect
    {
        public int TurnsRemaining { get; set; }
        public abstract string EffectName { get; }
        public bool IsExpired => TurnsRemaining <= 0;
        protected abstract void OnTurnStart(HealthController healthController);

        public void TickEffect(HealthController healthController)
        {
            OnTurnStart(healthController);
            TurnsRemaining--;
        }
        protected StatusEffect(int duration) => TurnsRemaining = duration;
        
        
    }
}