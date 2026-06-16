using Entities;

namespace StatusEffects
{
    public class StunEffect : StatusEffect
    {
        public override string EffectName => "Stun";

        public StunEffect(int duration) : base(duration) { }

        protected override void OnTurnStart(HealthController healthController)
        {
            // stun is checked in TurnSystem/Controllers, not here
        }
    }
}