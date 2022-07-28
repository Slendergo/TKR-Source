using wServer.core;
using wServer.core.objects;

namespace wServer.logic.transitions
{
    internal class NoPlayerWithinTransition : Transition
    {
        //State storage: none

        private readonly double dist;

        public NoPlayerWithinTransition(double dist, string targetState)
            : base(targetState)
        {
            this.dist = dist;
        }

        protected override bool TickCore(Entity host, TickData time, ref object state)
        {
            return host.GetNearestEntity(dist, null) == null;
        }
    }
}
