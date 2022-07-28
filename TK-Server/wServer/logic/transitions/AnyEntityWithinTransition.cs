using wServer.core;
using wServer.core.objects;

namespace wServer.logic.transitions
{
    internal class AnyEntityWithinTransition : Transition
    {
        //State storage: none

        private readonly int _dist;

        public AnyEntityWithinTransition(int dist, string targetState)
            : base(targetState)
        {
            _dist = dist;
        }

        protected override bool TickCore(Entity host, TickData time, ref object state)
        {
            return host.AnyEnemyNearby(_dist) || host.AnyPlayerNearby(_dist);
        }
    }
}
