using wServer.core;
using wServer.core.objects;

namespace wServer.logic.behaviors
{
    internal class Decay : Behavior
    {
        private readonly int time;

        public Decay(int time = 10000) => this.time = time;

        protected override void OnStateEntry(Entity host, TickData time, ref object state) => state = this.time;

        protected override void TickCore(Entity host, TickData time, ref object state)
        {
            var cool = (int)state;

            if (cool <= 0)
                host.Owner.LeaveWorld(host);
            else
                cool -= time.ElaspedMsDelta;

            state = cool;
        }
    }
}
