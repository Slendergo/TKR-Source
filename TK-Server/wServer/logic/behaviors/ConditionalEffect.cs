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

        protected override void OnStateEntry(Entity host, TickTime time, ref object state)
        {
            host.ApplyConditionEffect(effect, duration);
        }

        protected override void OnStateExit(Entity host, TickTime time, ref object state)
        {
            if(!perm)
                host.ApplyConditionEffect(effect, 0);
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
        }
    }
}
