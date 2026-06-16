using Entities;

namespace StatusEffects
{
    public class PoisonEffect : StatusEffect
    {
        public override string EffectName => "Poison";
        private readonly int _damagePerTurn;

        public PoisonEffect(int damagePerTurn, int duration) : base(duration)
        {
            _damagePerTurn = damagePerTurn;
        }

        protected override void OnTurnStart(HealthController healthController)
        {
            healthController.TakeDamage(_damagePerTurn, DamageType.Physical);
        }
    }
}