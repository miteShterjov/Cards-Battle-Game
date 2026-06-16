using Entities;

namespace StatusEffects
{
    public class WeaknessEffect : StatusEffect
    {
        public override string EffectName => "Weakness";
        public float DamageMultiplier => 0.5f; // 50% damage reduction

        public WeaknessEffect(int duration) : base(duration) { }

        protected override void OnTurnStart(HealthController healthController)
        {
            // weakness is checked at damage application time, not on turn start
            // nothing ticks here, just counts down
        }
    }
}