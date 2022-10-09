using TKR.Shared.resources;
using TKR.WorldServer.core.miscfile.thread;
using TKR.WorldServer.core.objects;

namespace TKR.WorldServer.logic.behaviors
{
    internal class ConditionEffectBehavior : Behavior
    {
        private readonly bool perm;
        private int duration;
        private ConditionEffectIndex effect;

        public ConditionEffectBehavior(ConditionEffectIndex effect, bool perm = false, int duration = -1)
        {
            this.effect = effect;
            this.perm = perm;
            this.duration = duration;
        }

        protected override void OnStateEntry(Entity host, TickTime time, ref object state)
        {
            System.Console.WriteLine($"[OnStateEntry ({time.TickCount})] ApplyConditionEffect({effect}, {duration})");
            host.ApplyConditionEffect(effect, duration);
        }

        protected override void OnStateExit(Entity host, TickTime time, ref object state)
        {
            System.Console.WriteLine($"[OnStateExit ({time.TickCount})] Perm:{perm} | RemoveCondition({effect})");
            if (!perm)
                host.RemoveCondition(effect);
        }

        protected override void TickCore(Entity host, TickTime time, ref object state)
        {
        }
    }
}
