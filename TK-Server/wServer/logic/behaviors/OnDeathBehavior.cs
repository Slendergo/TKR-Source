using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    public class OnDeathBehavior : Behavior
    {
        private readonly Behavior behavior;

        public OnDeathBehavior(Behavior behavior) => this.behavior = behavior;

        protected internal override void Resolve(State parent) => parent.Death += (s, e) => behavior.OnStateEntry(e.Host, e.Time);

        protected override void TickCore(Entity host, TickData time, ref object state)
        { }
    }
}
