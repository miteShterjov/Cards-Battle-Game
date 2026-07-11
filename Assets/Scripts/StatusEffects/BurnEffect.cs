using Cards;
using Entities;

namespace StatusEffects
{
    public class BurnEffect : StatusEffect
    {
        public override string EffectName => "Burn";
        private readonly int _damagePerTurn;

        public BurnEffect(int damagePerTurn, int duration) : base(duration)
        {
            _damagePerTurn = damagePerTurn;
        }

        protected override void OnTurnStart(HealthController healthController)
        {
            // ignores armor entirely because it's magical damage
            healthController.TakeDamage(_damagePerTurn, DamageType.Magical); 
        }
    }
}