using common.resources;
using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class ConditionalEffect : Behavior
    {
        private readonly bool perm;
        private int duration;
        private ConditionEffectIndex effect;

        public ConditionalEffect(ConditionEffectIndex effect, bool perm = false, int duration = -1)
        {
            this.effect = effect;
            this.perm = perm;
            this.duration = duration;
        }

        protected override void OnStateEntry(Entity host, TickData time, ref object state) => host.ApplyConditionEffect(new ConditionEffect()
        { Effect = effect, DurationMS = duration });

        protected override void OnStateExit(Entity host, TickData time, ref object state)
        {
            if (!perm)
                host.ApplyConditionEffect(new ConditionEffect() { Effect = effect, DurationMS = 0 });
        }

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
