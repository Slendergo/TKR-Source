using wServer.core;
using wServer.core.objects;

namespace wServer.logic.transitions
{
    internal class PlayerWithinTransition : Transition
    {
        //State storage: none

        private readonly double _dist;
        private readonly bool _seeInvis;

        public PlayerWithinTransition(double dist, string targetState, bool seeInvis = false)
            : base(targetState)
        {
            _dist = dist;
            _seeInvis = seeInvis;
        }

        protected override bool TickCore(Entity host, TickData time, ref object state)
        {
            return host.GetNearestEntity(_dist, null, _seeInvis) != null;
        }
    }
}
