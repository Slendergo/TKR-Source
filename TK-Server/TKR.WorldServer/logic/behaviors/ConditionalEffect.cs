using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.logic;

namespace TKR.WorldServer.logic.behaviors
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
            if (!perm)
                host.RemoveCondition(effect);
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
        }
    }
}
