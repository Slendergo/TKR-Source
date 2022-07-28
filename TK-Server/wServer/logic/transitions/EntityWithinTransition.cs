using wServer.core;
using wServer.core.objects;

namespace wServer.logic.transitions
{
    internal class EntityWithinTransition : Transition
    {
        //State storage: none

        private readonly double _dist;
        private readonly string _entity;

        public EntityWithinTransition(double dist, string entity, string targetState)
            : base(targetState)
        {
            _dist = dist;
            _entity = entity;
        }

        protected override bool TickCore(Entity host, TickData time, ref object state)
        {
            return host.GetNearestEntityByName(_dist, _entity) != null;
        }
    }
}
